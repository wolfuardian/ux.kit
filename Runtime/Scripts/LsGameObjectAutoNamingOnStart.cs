using System.Collections.Generic;
using UnityEngine;

namespace Eos.Ux.Kit
{
    public class LsGameObjectAutoNamingOnStart : MonoBehaviour
    {
        [SerializeField] private string _prefix = "AutoNamed_";
        [SerializeField] private string _suffix = "_v1";
        [SerializeField] private List<GameObjectMiddleName> _gameObjectList = new List<GameObjectMiddleName>();

        private void Start()
        {
            _gameObjectList.ForEach(target => target.gameObject.name = GenerateUniqueName(target.middleName));
        }

        private string GenerateUniqueName(string middleName)
        {
            var uniqueName = _prefix + middleName + _suffix;
            var counter    = 1;

            while (GameObject.Find(uniqueName) != null)
            {
                uniqueName = _prefix + middleName + _suffix + "_" + counter;
                counter++;
            }

            return uniqueName;
        }

        [System.Serializable]
        private class GameObjectMiddleName
        {
            public string middleName;
            public GameObject gameObject;
        }
    }
}
