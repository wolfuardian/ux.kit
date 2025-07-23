using UnityEngine;

namespace Eos.Ux.Kit
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LsRectTrackMousePosition : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 _screenOffset;

        private RectTransform _rectTransform;
        public RectTransform CachedRectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
 
        public Canvas Canvas
        {
            get
            {
                if (_canvas == null) _canvas = GetComponentInParent<Canvas>();
                return _canvas;
            }
        }

        private static Vector2 ScreenScaleRatio
        {
            get
            {
                return new Vector2(Screen.width / 1920f, Screen.height / 1080f);
            }
        }

        private void LateUpdate()
        {
            TrackMouse();
        }

        private void TrackMouse()
        {
            CachedRectTransform.pivot = _pivot;

            if (Canvas == null)
                return;

            Vector2 mousePos = Input.mousePosition;

            var screenCenter = GetScreenCenter();

            var offsetPos = mousePos - screenCenter + _screenOffset * ScreenScaleRatio;

            var newAnchoredPos = offsetPos / Canvas.scaleFactor;

            if (CachedRectTransform.anchoredPosition != newAnchoredPos)
            {
                CachedRectTransform.anchoredPosition = newAnchoredPos;
            }
        }

        private Vector2 GetScreenCenter()
        {
            if (Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return Canvas.pixelRect.size / 2f;
            }
            return Canvas.GetComponent<RectTransform>().sizeDelta / 2f;
        }
    }
}
