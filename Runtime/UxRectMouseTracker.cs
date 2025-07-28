using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX/Kit/UX Rect Mouse Tracker")]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class UxRectMouseTracker : MonoBehaviour
    {
        [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 _screenOffset;

        public static Vector2 screenScaleRatio => new Vector2(Screen.width / 1920f, Screen.height / 1080f);

        private RectTransform _rectTransform;
        private RectTransform cachedRectTransform => _rectTransform ??= GetComponent<RectTransform>();

        private Canvas _canvas;
        private Canvas cachedCanvas => _canvas ??= GetComponentInParent<Canvas>();

        private void LateUpdate()
        {
            TrackMouse();
        }

        private void TrackMouse()
        {
            cachedRectTransform.pivot = _pivot;

            if (cachedCanvas == null)
                return;

            Vector2 mousePos = Input.mousePosition;

            var screenCenter = GetScreenCenter();

            var offsetPos = mousePos - screenCenter + _screenOffset * screenScaleRatio;

            var newAnchoredPos = offsetPos / cachedCanvas.scaleFactor;

            if (cachedRectTransform.anchoredPosition != newAnchoredPos)
            {
                cachedRectTransform.anchoredPosition = newAnchoredPos;
            }
        }

        private Vector2 GetScreenCenter()
        {
            if (cachedCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return cachedCanvas.pixelRect.size / 2f;
            }
            return cachedCanvas.GetComponent<RectTransform>().sizeDelta / 2f;
        }
    }
}
