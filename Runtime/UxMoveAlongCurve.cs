using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX Move Along Curve")]
    public class UxMoveAlongCurve : MonoBehaviour
    {
        public enum MoveMode { Once, Loop, PingPong }

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

            var lr          = _lineRenderer;
            var localOffset = lr.useWorldSpace ? Vector3.zero : lr.transform.position;
            transform.position = GetPositionAlongLine(lr, _t, GetTotalLength(lr)) + localOffset;

            var scaledT = _scaleCurve.Evaluate(_t);
            transform.localScale = _cachedLocalScale * scaledT;
            _onPositionChanged?.Invoke(scaledT);
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
            var     targetLength = t * totalLength;
            for (var i = 0; i < line.positionCount - 1; i++)
            {
                var segmentLength = Vector3.Distance(line.GetPosition(i), line.GetPosition(i + 1));
                if (targetLength <= segmentLength)
                {
                    position = Vector3.Lerp(line.GetPosition(i), line.GetPosition(i + 1), targetLength / segmentLength);
                    position.Scale(line.transform.lossyScale);
                    return position;
                }
                targetLength -= segmentLength;
            }
            position = line.GetPosition(line.positionCount - 1);
            position.Scale(line.transform.lossyScale);
            return position;
        }
    }
}
