using System.Collections.Generic;
using UnityEngine;

namespace Ux.Kit
{
    public class LsGameObjectSwitcherInt : MonoBehaviour
    {
        [SerializeField] private int _switchInt = 0;
        [SerializeField] private List<GameObject> _gameObjects = new List<GameObject>();

        public void Switch(int index)
        {
            _gameObjects.ForEach(go => go.SetActive(false));

            if (index >= 0 && index < _gameObjects.Count)
            {
                _gameObjects[index].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Index out of range: " + index);
            }
        }

        private void Start()
        {
            if (_gameObjects == null || _gameObjects.Count == 0)
            {
                Debug.LogWarning("No GameObjects provided. Please add GameObjects to the list.");
                return;
            }

            // Initialize by activating the first GameObject
            Switch(_switchInt);
        }
    }
}
