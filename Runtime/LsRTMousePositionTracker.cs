using UnityEngine;

namespace Ux.Kit
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LsRTMousePositionTracker : MonoBehaviour
    {
        [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 _screenOffset;

        public Vector2 ScreenScaleRatio => new Vector2(Screen.width / 1920f, Screen.height / 1080f);

        private RectTransform _rectTransform;
        private RectTransform CachedRectTransform => _rectTransform ??= GetComponent<RectTransform>();

        private Canvas _canvas;
        private Canvas CachedCanvas => _canvas ??= GetComponentInParent<Canvas>();

        private void LateUpdate()
        {
            TrackMouse();
        }

        private void TrackMouse()
        {
            CachedRectTransform.pivot = _pivot;

            if (CachedCanvas == null)
                return;

            Vector2 mousePos = Input.mousePosition;

            var screenCenter = GetScreenCenter();

            var offsetPos = mousePos - screenCenter + _screenOffset * ScreenScaleRatio;

            var newAnchoredPos = offsetPos / CachedCanvas.scaleFactor;

            if (CachedRectTransform.anchoredPosition != newAnchoredPos)
            {
                CachedRectTransform.anchoredPosition = newAnchoredPos;
            }
        }

        private Vector2 GetScreenCenter()
        {
            if (CachedCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return CachedCanvas.pixelRect.size / 2f;
            }
            return CachedCanvas.GetComponent<RectTransform>().sizeDelta / 2f;
        }
    }
}
