using UnityEngine;
using UnityEngine.Events;

namespace Ux.Kit
{
    [AddComponentMenu("UX/Kit/UX Event Dispatcher")]
    public class UxEventDispatcher : MonoBehaviour
    {
        [SerializeField] private string _eventID;

        public UnityAction<string> onEvent = delegate {};

        public void DispatchEvent()
        {
            onEvent.Invoke(_eventID);
        }
    }
}
