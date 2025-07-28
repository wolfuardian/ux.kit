using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ux.Kit
{
    public class LsTextsSetter : MonoBehaviour
    {
        [SerializeField] private List<TextSettings> _textsList = new List<TextSettings>();

        private void Start()
        {
            if (_textsList == null || _textsList.Count == 0)
            {
                Debug.LogWarning("No text settings provided. Please add TextSettings to the list.");
                return;
            }

            foreach (var textSettings in _textsList)
            {
                if (textSettings == null)
                {
                    Debug.LogWarning("A TextSettings object is null in the list.");
                    continue;
                }

                if (textSettings._textComponent == null)
                {
                    Debug.LogWarning("Text component is not assigned in TextSettings.");
                    continue;
                }

                textSettings.UpdateText();
            }
        }

        [System.Serializable]
        public class TextSettings
        {
            public Text _textComponent;
            public string _textContent;

            public void UpdateText()
            {
                _textComponent.text = _textContent;
            }
        }
    }
}
