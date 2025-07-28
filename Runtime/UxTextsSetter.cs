using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ux.Kit
{
    [AddComponentMenu("UX/Kit/UX Texts Setter")]
    public class UxTextsSetter : MonoBehaviour
    {
        [SerializeField] private List<TextSettings> _textComponents = new List<TextSettings>();

        private void Start()
        {
            if (_textComponents == null || _textComponents.Count == 0)
            {
                Debug.LogWarning("No text settings provided. Please add TextSettings to the list.");
                return;
            }

            foreach (var settings in _textComponents)
            {
                if (settings == null)
                {
                    Debug.LogWarning("A TextSettings object is null in the list.");
                    continue;
                }

                if (settings._target == null)
                {
                    Debug.LogWarning("Text component is not assigned in TextSettings.");
                    continue;
                }

                settings.UpdateText();
            }
        }

        [System.Serializable]
        public class TextSettings
        {
            public Text _target;
            public string content;

            public void UpdateText()
            {
                _target.text = content;
            }
        }
    }
}
