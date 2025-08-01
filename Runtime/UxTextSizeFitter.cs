using UnityEngine;
using UnityEngine.UI;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Text Size Fitter")]
    [RequireComponent(typeof(RectTransform), typeof(Text))]
    [DisallowMultipleComponent]
    public class UxTextSizeFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetRectTransform;
        [SerializeField] private Vector2 _sizeOffset = new Vector2(0.0f, 0.0f);
        [SerializeField] private bool _width = true;
        [SerializeField] private bool _height = false;

        private Text _text;
        private Text cachedText => _text ??= GetComponent<Text>();

        private void Update()
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            var target = _targetRectTransform;
            var scale  = cachedText.transform.localScale;
            var preferredSize = new Vector2(
                _width
                    ? (cachedText.preferredWidth + _sizeOffset.x) * scale.x
                    : target.sizeDelta.x,
                _height
                    ? (cachedText.preferredHeight + _sizeOffset.y) * scale.y
                    : target.sizeDelta.y
            );
            target.sizeDelta = preferredSize;
        }
    }
}
