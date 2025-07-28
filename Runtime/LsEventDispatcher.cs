using UnityEngine;
using UnityEngine.Events;

namespace Ux.Kit
{
    public class LsEventDispatcher : MonoBehaviour
    {
        [SerializeField] private string _eventID;

        public UnityAction<string> onEvent = delegate {};

        public void DispatchEvent()
        {
            onEvent.Invoke(_eventID);
        }
    }
}
