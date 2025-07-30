using System;
using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Draw Circle")]
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class UxDrawCircle : MonoBehaviour
    {
        private enum AxisUp
        {
            X,
            Y,
            Z
        }

        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _lineWidth = 0.1f;
        [SerializeField] private int _segments = 100;
        [SerializeField] private AxisUp _axisUp = AxisUp.Z;

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
            Draw();
        }

        #endif

        private void Start()
        {
            Draw();
        }

        private void UpdateRadius(float newRadius)
        {
            _radius = newRadius;
            Draw();
        }

        private void UpdateLineWidth(float newLineWidth)
        {
            _lineWidth = newLineWidth;
            Draw();
        }

        private void UpdateSegments(int newSegments)
        {
            _segments = newSegments;
            Draw();
        }

        private void ClampValues()
        {
            _segments = Mathf.Max(3, _segments);
            _radius = Mathf.Max(0.01f, _radius);
            _lineWidth = Mathf.Max(0.01f, _lineWidth);
        }

        private void Draw()
        {
            ClampValues();

            cachedLineRenderer.positionCount = _segments + 1;
            cachedLineRenderer.startWidth = _lineWidth;
            cachedLineRenderer.endWidth = _lineWidth;
            cachedLineRenderer.useWorldSpace = false;

            switch (_axisUp)
            {
                case AxisUp.X:
                    for (var i = 0; i <= _segments; i++)
                    {
                        var angle = i * 2 * Mathf.PI / _segments;
                        var y     = Mathf.Cos(angle) * _radius;
                        var z     = Mathf.Sin(angle) * _radius;
                        cachedLineRenderer.SetPosition(i, new Vector3(0, y, z));
                    }
                    break;

                case AxisUp.Y:
                    for (var i = 0; i <= _segments; i++)
                    {
                        var angle = i * 2 * Mathf.PI / _segments;
                        var x     = Mathf.Cos(angle) * _radius;
                        var z     = Mathf.Sin(angle) * _radius;
                        cachedLineRenderer.SetPosition(i, new Vector3(x, 0, z));
                    }
                    break;

                case AxisUp.Z:
                    for (var i = 0; i <= _segments; i++)
                    {
                        var angle = i * 2 * Mathf.PI / _segments;
                        var x     = Mathf.Cos(angle) * _radius;
                        var y     = Mathf.Sin(angle) * _radius;
                        cachedLineRenderer.SetPosition(i, new Vector3(x, y, 0));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
