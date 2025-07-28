using System.Collections.Generic;
using UnityEngine;

namespace Ux.Kit
{
    public class LsGameObjectsNameSetter : MonoBehaviour
    {
        [SerializeField] private string _prefix = "Prefix_";
        [SerializeField] private string _suffix = "_Suffix";
        [SerializeField] private List<GameObjectSettings> _gameObjectList = new List<GameObjectSettings>();

        private void Start()
        {
            if (_gameObjectList == null || _gameObjectList.Count == 0)
            {
                Debug.LogWarning("No GameObject settings provided. Please add GameObjectSettings to the list.");
                return;
            }

            foreach (var gameObjectSettings in _gameObjectList)
            {
                if (gameObjectSettings == null || gameObjectSettings._targetGameObject == null)
                {
                    Debug.LogWarning("A GameObjectSettings object or its target GameObject is null in the list.");
                    continue;
                }

                gameObjectSettings.UpdateName(_prefix, _suffix);
            }
        }

        [System.Serializable]
        private class GameObjectSettings
        {
            public GameObject _targetGameObject;
            public string _middleName;

            public void UpdateName(string prefix, string suffix)
            {
                var uniqueName = prefix + _middleName + suffix;
                var counter    = 1;

                while (GameObject.Find(uniqueName) != null)
                {
                    uniqueName = prefix + _middleName + suffix + "_" + counter;
                    counter++;
                }

                _targetGameObject.name = uniqueName;
            }
        }
    }
}
