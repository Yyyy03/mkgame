using System;
using System.Text.Json;
using MKGame.World.State;
using MKGame.Events;

namespace MKGame.Save
{
    public sealed class SaveSystem
    {
        private readonly JsonSerializerOptions _options;

        public SaveSystem()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
        }

        public string Serialize(WorldState state)
        {
            var dto = ToDto(state);
            return JsonSerializer.Serialize(dto, _options);
        }

        public WorldState Deserialize(string json)
        {
            var dto = JsonSerializer.Deserialize<WorldStateDto>(json, _options);
            return FromDto(dto);
        }

        private WorldStateDto ToDto(WorldState state)
        {
            var map = new MapStateDto
            {
                Width = state.Map.Width,
                Height = state.Map.Height,
                Tiles = new TileDto[state.Map.Tiles != null ? state.Map.Tiles.Length : 0],
                Biomes = new BiomeDto[state.Map.Biomes != null ? state.Map.Biomes.Length : 0],
                Resources = new ResourceDto[state.Map.Resources != null ? state.Map.Resources.Count : 0]
            };

            if (state.Map.Tiles != null)
            {
                for (var i = 0; i < state.Map.Tiles.Length; i++)
                {
                    map.Tiles[i] = new TileDto
                    {
                        TileType = state.Map.Tiles[i].TileType,
                        Flags = state.Map.Tiles[i].Flags
                    };
                }
            }

            if (state.Map.Biomes != null)
            {
                for (var i = 0; i < state.Map.Biomes.Length; i++)
                {
                    map.Biomes[i] = new BiomeDto
                    {
                        BiomeType = state.Map.Biomes[i].BiomeType
                    };
                }
            }

            if (state.Map.Resources != null)
            {
                for (var i = 0; i < state.Map.Resources.Count; i++)
                {
                    var res = state.Map.Resources[i];
                    map.Resources[i] = new ResourceDto
                    {
                        Id = res.Id,
                        Type = res.Type,
                        Amount = res.Amount,
                        X = res.Position.x,
                        Y = res.Position.y
                    };
                }
            }

            var entities = new EntityStateDto
            {
                Entities = new System.Collections.Generic.List<EntityDto>(state.Entities.Entities.Count)
            };

            foreach (var kv in state.Entities.Entities)
            {
                var data = kv.Value;
                entities.Entities.Add(new EntityDto
                {
                    Id = data.Id.Value,
                    X = data.Position.x,
                    Y = data.Position.y,
                    Hp = data.Hp,
                    Energy = data.Energy,
                    Faction = data.Faction
                });
            }

            var simulation = new SimulationStateDto
            {
                TimeOfDay = state.Simulation.TimeOfDay,
                SeasonIndex = state.Simulation.SeasonIndex,
                EcoPopulation = state.Simulation.EcoPopulation
            };

            var eventsDto = new EventStateDto
            {
                Queue = new System.Collections.Generic.List<EventDto>()
            };

            foreach (var evt in state.Events.EventQueue)
            {
                eventsDto.Queue.Add(ToEventDto(evt));
            }

            return new WorldStateDto
            {
                Map = map,
                Entities = entities,
                Simulation = simulation,
                Events = eventsDto,
                Rng = state.Rng,
                Tick = state.Tick
            };
        }

        private WorldState FromDto(WorldStateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var state = new WorldState();
            state.Map.Width = dto.Map.Width;
            state.Map.Height = dto.Map.Height;
            state.Map.Tiles = new TileData[dto.Map.Tiles != null ? dto.Map.Tiles.Length : 0];
            state.Map.Biomes = new BiomeData[dto.Map.Biomes != null ? dto.Map.Biomes.Length : 0];

            if (dto.Map.Tiles != null)
            {
                for (var i = 0; i < dto.Map.Tiles.Length; i++)
                {
                    state.Map.Tiles[i] = new TileData
                    {
                        TileType = dto.Map.Tiles[i].TileType,
                        Flags = dto.Map.Tiles[i].Flags
                    };
                }
            }

            if (dto.Map.Biomes != null)
            {
                for (var i = 0; i < dto.Map.Biomes.Length; i++)
                {
                    state.Map.Biomes[i] = new BiomeData
                    {
                        BiomeType = dto.Map.Biomes[i].BiomeType
                    };
                }
            }

            if (dto.Map.Resources != null)
            {
                for (var i = 0; i < dto.Map.Resources.Length; i++)
                {
                    var res = dto.Map.Resources[i];
                    state.Map.Resources.Add(new ResourceNode
                    {
                        Id = res.Id,
                        Type = res.Type,
                        Amount = res.Amount,
                        Position = new UnityEngine.Vector2Int(res.X, res.Y)
                    });
                }
            }

            if (dto.Entities != null && dto.Entities.Entities != null)
            {
                for (var i = 0; i < dto.Entities.Entities.Count; i++)
                {
                    var ent = dto.Entities.Entities[i];
                    var id = new EntityId(ent.Id);
                    state.Entities.Entities[id] = new EntityData
                    {
                        Id = id,
                        Position = new UnityEngine.Vector2Int(ent.X, ent.Y),
                        Hp = ent.Hp,
                        Energy = ent.Energy,
                        Faction = ent.Faction
                    };
                }
            }

            state.Simulation.TimeOfDay = dto.Simulation.TimeOfDay;
            state.Simulation.SeasonIndex = dto.Simulation.SeasonIndex;
            state.Simulation.EcoPopulation = dto.Simulation.EcoPopulation;

            if (dto.Events != null && dto.Events.Queue != null)
            {
                for (var i = 0; i < dto.Events.Queue.Count; i++)
                {
                    var evt = FromEventDto(dto.Events.Queue[i]);
                    if (evt != null)
                    {
                        state.Events.EventQueue.Enqueue(evt);
                    }
                }
            }

            state.Rng = dto.Rng;
            state.Tick = dto.Tick;
            return state;
        }

        public EventDto ToEventDto(GameEvent evt)
        {
            var type = evt.GetType();
            var json = JsonSerializer.Serialize(evt, type, _options);
            using var doc = JsonDocument.Parse(json);
            var payload = doc.RootElement.Clone();
            return new EventDto
            {
                Type = type.Name,
                Payload = payload
            };
        }

        public GameEvent FromEventDto(EventDto dto)
        {
            var type = EventTypeRegistry.GetTypeByName(dto.Type);
            if (type == null)
            {
                throw new NotSupportedException("Unknown event type: " + dto.Type);
            }

            var json = dto.Payload.GetRawText();
            return (GameEvent)JsonSerializer.Deserialize(json, type, _options);
        }
    }
}
