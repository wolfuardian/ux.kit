using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX GameObject Name Setter")]
    public class UxGameObjectNameSetter : MonoBehaviour
    {
        [SerializeField] private GameObject _targetGameObject;
        [SerializeField] private string _prefix = "Prefix_";
        [SerializeField] private string _middleName = "MiddleName";
        [SerializeField] private string _suffix = "_Suffix";

        private void Start()
        {
            if (_targetGameObject == null)
            {
                _targetGameObject = gameObject;
            }

            if (_targetGameObject != null)
            {
                UpdateName();
            }
            else
            {
                Debug.LogWarning("Target GameObject is not assigned or found on the GameObject.");
            }
        }

        private void UpdateName()
        {
            var uniqueName = _prefix + _middleName + _suffix;
            var counter    = 1;

            while (GameObject.Find(uniqueName) != null)
            {
                uniqueName = _prefix + _middleName + _suffix + "_" + counter;
                counter++;
            }

            _targetGameObject.name = uniqueName;
        }
    }
}
