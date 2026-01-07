using UnityEngine;
using MKGame.Core.Time;

namespace MKGame.Core.Time
{
    public sealed class TickDriver : MonoBehaviour
    {
        private ITickScheduler _scheduler;

        private void Start()
        {
            _scheduler = MKGame.Core.GameRoot.Instance.TickScheduler;
        }

        private void Update()
        {
            _scheduler.Tick(Time.deltaTime);
        }
    }
}