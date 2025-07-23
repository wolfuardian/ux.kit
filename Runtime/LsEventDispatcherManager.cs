using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Eos.Ux.Kit
{
    public class LsEventDispatcherManager : MonoBehaviour
    {
        [SerializeField] private List<EventData> _events = new List<EventData>();

        private readonly Dictionary<string, UnityEvent> _eventMap = new Dictionary<string, UnityEvent>();

        private void Awake()
        {
            BuildEventMap();
            RegisterAllDispatchers();
        }

        private void BuildEventMap()
        {
            _eventMap.Clear();
            foreach (var e in _events)
            {
                _eventMap[e._eventID] = e._onEvent;
            }
        }

        private void RegisterAllDispatchers()
        {
            #if UNITY_2022_1_OR_NEWER
            var dispatchers = FindObjectsByType<LsEventDispatcher>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            #else
            var dispatchers = FindObjectsOfType<LsEventDispatcher>(true);
            #endif
            foreach (var d in dispatchers)
            {
                d.onEvent += HandleEvent;
            }
        }

        private void HandleEvent(string id)
        {
            if (_eventMap.TryGetValue(id, out var evt))
                evt.Invoke();
        }

        [System.Serializable]
        public class EventData
        {
            public string _eventID;
            public UnityEvent _onEvent;
        }
    }
}
