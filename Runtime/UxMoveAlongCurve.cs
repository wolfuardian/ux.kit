using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Move Along Curve")]
    public class UxMoveAlongCurve : MonoBehaviour
    {
        private enum MoveMode { Once, Loop, PingPong }
        
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _duration = 5f;
        [SerializeField] private MoveMode _mode = MoveMode.Loop;
        [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
        [SerializeField] private UnityEvent<float> _onPositionChanged;

        private float _t = 0f;
        private bool _movingForward = true;
        private Vector3 _cachedLocalScale = Vector3.one;

        private void Start()
        {
            _cachedLocalScale = transform.localScale;
            transform.position = _lineRenderer.GetPosition(0);
        }

        private void Update()
        {
            var speed = 1f / _duration;
            _t += speed * Time.deltaTime * (_movingForward ? 1 : -1);

            switch (_mode)
            {
                case MoveMode.Once:
                    if (_t >= 1f)
                    {
                        _t = 1f;
                        enabled = false;
                    }
                    break;
                case MoveMode.Loop:
                    if (_t >= 1f) _t -= 1f;
                    else if (_t < 0) _t += 1f;
                    break;
                case MoveMode.PingPong:
                    if (_t is >= 1f or <= 0)
                    {
                        _movingForward = !_movingForward;
                        _t = Mathf.Clamp(_t, 0, 1f);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var lr = _lineRenderer;
            var localPoint = GetPositionAlongLine(lr, _t, GetTotalLength(lr));
            transform.position = GetWorldPosition(lr, localPoint);
            transform.eulerAngles = Quaternion.LookRotation(GetTangentAt(lr, _t), Vector3.up).eulerAngles;

            var scaledT = _scaleCurve.Evaluate(_t);
            transform.localScale = _cachedLocalScale * scaledT;
            _onPositionChanged?.Invoke(scaledT);
        }

        private static Vector3 GetWorldPosition(LineRenderer lr, Vector3 point)
        {
            return lr.useWorldSpace ? point : lr.transform.TransformPoint(point);
        }

        private static float GetTotalLength(LineRenderer line)
        {
            var totalLength   = 0f;
            var positionCount = line.positionCount;
            for (var i = 0; i < positionCount - 1; i++)
            {
                totalLength += Vector3.Distance(line.GetPosition(i), line.GetPosition(i + 1));
            }
            return totalLength;
        }

        private static Vector3 GetPositionAlongLine(LineRenderer line, float t, float totalLength)
        {
            Vector3 position;
            var     positionCount = line.positionCount;
            var     targetLength  = t * totalLength;
            for (var i = 0; i < positionCount - 1; i++)
            {
                var segmentLength = Vector3.Distance(line.GetPosition(i), line.GetPosition(i + 1));
                if (targetLength <= segmentLength)
                {
                    position = Vector3.Lerp(line.GetPosition(i), line.GetPosition(i + 1), targetLength / segmentLength);
                    return position;
                }
                targetLength -= segmentLength;
            }
            position = line.GetPosition(positionCount - 1);
            return position;
        }

        private static Vector3 GetTangentAt(LineRenderer line, float t)
        {
            var pointCount = line.positionCount;
            if (pointCount < 2) return Vector3.zero;

            var totalLength    = 0f;
            var segmentLengths = new float[pointCount - 1];

            for (var i = 0; i < pointCount - 1; i++)
            {
                var len = Vector3.Distance(line.GetPosition(i), line.GetPosition(i + 1));
                segmentLengths[i] = len;
                totalLength += len;
            }

            var targetLength = t * totalLength;
            var accumulated  = 0f;
            for (var i = 0; i < segmentLengths.Length; i++)
            {
                var segLen = segmentLengths[i];
                if (accumulated + segLen >= targetLength)
                {
                    var p0      = line.GetPosition(i);
                    var p1      = line.GetPosition(i + 1);
                    var tangent = (p1 - p0).normalized;
                    return tangent;
                }
                accumulated += segLen;
            }
            return (line.GetPosition(pointCount - 1) - line.GetPosition(pointCount - 2)).normalized;
        }
    }
}
