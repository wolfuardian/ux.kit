using UnityEngine;

namespace Eos.Ux.Kit
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LsRTScreenPointTracker : MonoBehaviour
    {
        [SerializeField] private Transform _trackTarget;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector3 _worldOffset;
        [SerializeField] private Vector2 _screenOffset;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _occludedAlpha = 0.25f;

        [SerializeField] private bool _snapToEdge = false;
        [SerializeField] private bool _shouldBeOccluded = false;

        private RectTransform _rectTransform;

        private float _storedAlpha;

        private Vector3 _track3DPos;
        private Vector3 _targetPos;
        private Vector2 _screenPoint;
        private Vector2 _screenCenter;

        public static Vector2 ScreenScaleRatio
        {
            get
            {
                return new Vector2(Screen.width / 1920f, Screen.height / 1080f);
            }
        }

        public RectTransform CachedRectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        public Camera CachedCamera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }

        public Canvas CachedCanvas
        {
            get
            {
                if (_canvas == null) _canvas = GetComponentInParent<Canvas>();
                return _canvas;
            }
        }

        public CanvasGroup CachedCanvasGroup
        {
            get
            {
                if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        public Transform TrackTarget
        {
            get
            {
                if (_trackTarget == null) _trackTarget = transform;
                return _trackTarget;
            }
            set
            {
                _trackTarget = value;
            }
        }

        public void SetTarget(Transform target)
        {
            TrackTarget = target;
        }

        public void SetTarget(GameObject target)
        {
            TrackTarget = target.transform;
        }

        public void SetTarget(Vector3 target)
        {
            TrackTarget.position = target;
        }

        public void Hide()
        {
            _screenPoint = new Vector2(-1000, -1000);
        }

        public void SetPivot(Vector2 pivot)
        {
            _pivot = pivot;
        }

        public void SetWorldOffset(Vector3 worldOffset)
        {
            _worldOffset = worldOffset;
        }

        public void SetScreenOffset(Vector2 screenOffset)
        {
            _screenOffset = screenOffset;
        }

        public Vector2 GetScreenCenter()
        {
            if (CachedCanvas == null)
            {
                return new Vector2(Screen.width / 2f, Screen.height / 2f);
            }
            return CachedCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? CachedCanvas.pixelRect.size / 2 : CachedCanvas.GetComponent<RectTransform>().sizeDelta / 2;
        }

        private void Start()
        {
            _track3DPos = Vector3.zero;
            Track();

            if (CachedCanvasGroup)
            {
                _storedAlpha = CachedCanvasGroup.alpha;
                return;
            }
            _shouldBeOccluded = false;
        }

        private void LateUpdate()
        {
            Track();
            Occluding();
        }

        private void Track()
        {
            CachedRectTransform.pivot = _pivot;

            if (TrackTarget == null || CachedCamera == null)
            {
                return;
            }

            _screenPoint = GetScreenPoint();
            _screenCenter = GetScreenCenter();

            var newPosition = (_screenPoint - _screenCenter) / CachedCanvas.scaleFactor;

            if (CachedRectTransform.anchoredPosition != newPosition)
            {
                CachedRectTransform.anchoredPosition = newPosition;
            }
            CachedRectTransform.anchoredPosition = (_screenPoint - _screenCenter) / CachedCanvas.scaleFactor;
        }

        private void Occluding()
        {
            if (_shouldBeOccluded)
            {
                var occluded = LsCameraHelper.IsOccluded(CachedCamera, _trackTarget.position);
                CachedCanvasGroup.alpha = occluded ? _occludedAlpha : _storedAlpha;
            }
        }

        private Vector2 GetScreenPoint()
        {
            _track3DPos = TrackTarget.position;
            _targetPos = _track3DPos + _worldOffset;
            var screenPoint = _snapToEdge ? LsCameraHelper.ClipWorldToScreenPoint(CachedCamera, _targetPos) : LsCameraHelper.WorldToScreenPoint(CachedCamera, _targetPos);
            screenPoint += _screenOffset * ScreenScaleRatio;
            return screenPoint;
        }
    }
}
