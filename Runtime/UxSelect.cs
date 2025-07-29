using UnityEngine;
using UnityEngine.Events;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Select")]
    public class UxSelect : MonoBehaviour
    {
        [SerializeField] private bool _isOn = false;

        [SerializeField] private UnityEvent _onSelected = new UnityEvent();
        [SerializeField] private UnityEvent _onDeselected = new UnityEvent();

        public void Toggle()
        {
            _isOn = !_isOn;
            if (_isOn)
            {
                Selecting();
            }
            else
            {
                Deselecting();
            }
        }

        public void Toggle(bool isOn)
        {
            _isOn = isOn;
            if (_isOn)
            {
                Selecting();
            }
            else
            {
                Deselecting();
            }
        }

        public void Selecting()
        {
            _isOn = true;
            _onSelected.Invoke();
        }

        public void Deselecting()
        {
            _isOn = false;
            _onDeselected.Invoke();
        }
    }
}
