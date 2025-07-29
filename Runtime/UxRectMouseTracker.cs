using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Rect Mouse Tracker")]
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
            if (cachedCanvas == null)
            {
                return;
            }

            cachedRectTransform.pivot = _pivot;
            Vector2 mousePosition = Input.mousePosition;
            var     screenCenter  = GetScreenCenter();
            var     offset        = mousePosition - screenCenter + _screenOffset * screenScaleRatio;
            var     newPosition   = offset / cachedCanvas.scaleFactor;
            var     oldPosition   = cachedRectTransform.anchoredPosition;
            if (oldPosition != newPosition)
            {
                cachedRectTransform.anchoredPosition = newPosition;
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
