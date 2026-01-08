using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MKGame.World.State;

namespace MKGame.Rendering
{
    public sealed class RenderSystem : MonoBehaviour
    {
        [Header("Tile Sprites (optional)")]
        public Sprite TileSpriteA;
        public Sprite TileSpriteB;

        [Header("Entity Sprites (optional)")]
        public Sprite PlayerSprite;
        public Sprite NpcSprite;
        [Header("Resource Sprites (optional)")]
        public Sprite ResourceSprite;
        [Header("Scene Effects")]
        public bool EnableDayNightTint = true;

        private WorldDiff _diff;
        private WorldState _state;
        private readonly Dictionary<EntityId, GameObject> _entityViews = new Dictionary<EntityId, GameObject>();
        private readonly Dictionary<int, GameObject> _resourceViews = new Dictionary<int, GameObject>();
        private Grid _grid;
        private Tilemap _tilemap;
        private Tile[] _tilePalette;
        private Sprite[] _tileSprites;
        private Sprite _entitySprite;
        private bool _initialized;

        private void Start()
        {
            var root = MKGame.Core.GameRoot.Instance;
            _diff = root.WorldDiff;
            _state = root.WorldState;
            root.EventBus.Subscribe<MKGame.Events.ResourcePickedEvent>(OnResourcePicked);
        }

        private void LateUpdate()
        {
            SyncStateIfChanged();
            if (!_initialized)
            {
                TryInitialize();
            }

            if (_initialized)
            {
                ApplyTileDiffs();
                ApplyEntityDiffs();
                ApplyDayNightTint();
            }

            // Consume diffs here and then clear exactly once.
            _diff.Clear();
        }

        private void TryInitialize()
        {
            if (_state.Map.Width <= 0 || _state.Map.Height <= 0)
            {
                return;
            }

            var tileSpriteA = TileSpriteA != null ? TileSpriteA : CreateSprite(new Color(0.35f, 0.35f, 0.35f));
            var tileSpriteB = TileSpriteB != null ? TileSpriteB : CreateSprite(new Color(0.25f, 0.4f, 0.25f));
            _tileSprites = new[] { tileSpriteA, tileSpriteB };
            _tilePalette = new Tile[_tileSprites.Length];
            for (var i = 0; i < _tileSprites.Length; i++)
            {
                var tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = _tileSprites[i];
                _tilePalette[i] = tile;
            }

            var fallbackEntity = CreateSprite(Color.white);
            if (PlayerSprite == null) PlayerSprite = fallbackEntity;
            if (NpcSprite == null) NpcSprite = fallbackEntity;
            _entitySprite = fallbackEntity;

            var gridObj = new GameObject("Grid");
            _grid = gridObj.AddComponent<Grid>();
            gridObj.transform.SetParent(transform, false);

            var tilemapObj = new GameObject("Tilemap");
            tilemapObj.transform.SetParent(gridObj.transform, false);
            _tilemap = tilemapObj.AddComponent<Tilemap>();
            tilemapObj.AddComponent<TilemapRenderer>();

            RenderFullMap();
            RenderAllEntities();
            RenderAllResources();

            _initialized = true;
        }

        private void RenderFullMap()
        {
            var width = _state.Map.Width;
            var height = _state.Map.Height;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), GetTileForIndex((y * width) + x));
                }
            }
        }

        private void RenderAllEntities()
        {
            foreach (var kv in _state.Entities.Entities)
            {
                CreateOrUpdateEntity(kv.Key, kv.Value);
            }
        }

        private void ApplyTileDiffs()
        {
            if (_diff.TileDiffs.Count == 0 || _tilemap == null)
            {
                return;
            }

            var width = _state.Map.Width;
            for (var i = 0; i < _diff.TileDiffs.Count; i++)
            {
                var diff = _diff.TileDiffs[i];
                var x = diff.Index % width;
                var y = diff.Index / width;
                _tilemap.SetTile(new Vector3Int(x, y, 0), GetTileForIndex(diff.Index));
            }
        }

        private void ApplyEntityDiffs()
        {
            if (_diff.EntityDiffs.Count == 0)
            {
                return;
            }

            for (var i = 0; i < _diff.EntityDiffs.Count; i++)
            {
                var diff = _diff.EntityDiffs[i];
                if (_state.Entities.Entities.TryGetValue(diff.Id, out var data))
                {
                    CreateOrUpdateEntity(diff.Id, data);
                }
            }
        }

        private void RenderAllResources()
        {
            _resourceViews.Clear();
            var sprite = ResourceSprite != null ? ResourceSprite : CreateSprite(new Color(0.8f, 0.8f, 0.2f));
            var list = _state.Map.Resources;
            for (var i = 0; i < list.Count; i++)
            {
                var node = list[i];
                var go = new GameObject("Resource_" + node.Id);
                var renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingOrder = 5;
                go.transform.position = new Vector3(node.Position.x, node.Position.y, -0.5f);
                _resourceViews[node.Id] = go;
            }
        }

        private void CreateOrUpdateEntity(EntityId id, EntityData data)
        {
            if (!_entityViews.TryGetValue(id, out var go) || go == null)
            {
                go = new GameObject("Entity_" + id.Value);
                var renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = _entitySprite;
                renderer.sortingOrder = 10;
                _entityViews[id] = go;
            }

            var sr = go.GetComponent<SpriteRenderer>();
            var isPlayer = id.Value == 1;
            sr.sprite = isPlayer ? PlayerSprite : NpcSprite;
            sr.color = Color.white;
            go.transform.position = new Vector3(data.Position.x, data.Position.y, -1f);
        }

        private void ApplyDayNightTint()
        {
            if (!EnableDayNightTint || _tilemap == null)
            {
                return;
            }

            var t = _state.Simulation.TimeOfDay;
            var isNight = t >= 18f || t < 6f;
            var tint = isNight ? new Color(0.6f, 0.7f, 0.9f) : Color.white;
            _tilemap.color = tint;
        }

        private void OnResourcePicked(MKGame.Events.ResourcePickedEvent evt)
        {
            if (_resourceViews.TryGetValue(evt.ResourceId, out var go) && go != null)
            {
                Destroy(go);
                _resourceViews.Remove(evt.ResourceId);
            }
        }

        private void SyncStateIfChanged()
        {
            var rootState = MKGame.Core.GameRoot.Instance.WorldState;
            if (!ReferenceEquals(_state, rootState))
            {
                _state = rootState;
                _initialized = false;
                if (_grid != null)
                {
                    Destroy(_grid.gameObject);
                }

                _entityViews.Clear();
                _resourceViews.Clear();
            }
        }

        private Tile GetTileForIndex(int index)
        {
            if (_tilePalette == null || _tilePalette.Length == 0)
            {
                return null;
            }

            var type = 0;
            if (_state.Map.Tiles != null && index >= 0 && index < _state.Map.Tiles.Length)
            {
                type = _state.Map.Tiles[index].TileType;
            }

            if (type < 0 || type >= _tilePalette.Length)
            {
                type = 0;
            }

            return _tilePalette[type];
        }

        private static Sprite CreateSprite(Color color)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.filterMode = FilterMode.Point;
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
    }
}
