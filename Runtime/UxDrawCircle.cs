using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Draw Circle")]
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class UxDrawCircle : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _lineWidth = 0.1f;
        [SerializeField] private int _segments = 100;

        public float radius
        {
            get => _radius;
            set => UpdateRadius(value);
        }

        public float lineWidth
        {
            get => _lineWidth;
            set => UpdateLineWidth(value);
        }

        public int segments
        {
            get => _segments;
            set => UpdateSegments(value);
        }

        private LineRenderer _lineRenderer;
        private LineRenderer cachedLineRenderer => _lineRenderer ??= GetComponent<LineRenderer>();

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            SetupLineRenderer();
            Draw();
        }
        #endif

        private void Start()
        {
            SetupLineRenderer();
            Draw();
        }

        private void UpdateRadius(float newRadius)
        {
            if (newRadius < 0.01f) newRadius = 0.01f;
            if (Mathf.Approximately(_radius, newRadius)) return;
            _radius = newRadius;
            Draw();
        }

        private void UpdateLineWidth(float newLineWidth)
        {
            if (newLineWidth < 0.01f) newLineWidth = 0.01f;
            if (Mathf.Approximately(_lineWidth, newLineWidth)) return;
            _lineWidth = newLineWidth;
            SetupLineRenderer();
        }

        private void UpdateSegments(int newSegments)
        {
            if (newSegments < 3) newSegments = 3;
            if (_segments == newSegments) return;
            _segments = newSegments;
            Draw();
        }

        private void SetupLineRenderer()
        {
            cachedLineRenderer.positionCount = _segments + 1;
            cachedLineRenderer.startWidth = _lineWidth;
            cachedLineRenderer.endWidth = _lineWidth;
            cachedLineRenderer.useWorldSpace = false;
        }

        private void Draw()
        {
            for (var i = 0; i <= _segments; i++)
            {
                var angle = i * 2 * Mathf.PI / _segments;
                var x     = Mathf.Cos(angle) * _radius;
                var y     = Mathf.Sin(angle) * _radius;
                cachedLineRenderer.SetPosition(i, new Vector3(x, y, 0));
            }
        }
    }
}
