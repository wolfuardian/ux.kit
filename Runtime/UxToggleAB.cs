using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Toggle AB")]
    public class UxToggleAB : MonoBehaviour
    {
        [SerializeField] private bool _isOn;
        [SerializeField] private GameObject _goA;
        [SerializeField] private GameObject _goB;

        public bool isOn
        {
            set
            {
                _isOn = value;
                ToggleActive();
            }
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            ToggleActive();
        }
        #endif

        private void ToggleActive()
        {
            if (_goA && _goA.activeSelf != _isOn)
            {
                _goA.SetActive(_isOn);
            }

            if (_goB && _goB.activeSelf != !_isOn)
            {
                _goB.SetActive(!_isOn);
            }
        }
    }
}
