using UnityEngine;
using UnityEngine.UI;

namespace Eos.Ux.Kit
{
    public class LsTextSetter : MonoBehaviour
    {
        [SerializeField] private Text _textComponent;
        [SerializeField] private string _textContent;

        private void Start()
        {
            if (_textComponent == null)
            {
                _textComponent = GetComponent<Text>();
            }

            if (_textComponent != null)
            {
                UpdateText();
            }
            else
            {
                Debug.LogWarning("Text component is not assigned or found on the GameObject.");
            }
        }

        private void UpdateText()
        {
            _textComponent.text = _textContent;
        }
    }
}
