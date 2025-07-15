using System.Collections.Generic;
using UnityEngine;

namespace Eos.Ux.Kit
{
    public class LsSwitchGameObject : MonoBehaviour
    {
        [SerializeField] private Settings _settings = new Settings();

        public void Switch(int index)
        {
            if (enabled)
            {
                _settings._gameObjects.ForEach(go => go.SetActive(false));
                if (index >= 0 && index < _settings._gameObjects.Count)
                {
                    _settings._gameObjects[index].SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Index out of range: " + index);
                }
            }
        }

        [System.Serializable]
        public class Settings
        {
            public List<GameObject> _gameObjects = new List<GameObject>();
        }
    }
}
