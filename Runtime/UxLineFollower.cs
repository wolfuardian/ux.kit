using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ux.Kit
{
    public class UxLineFollower : MonoBehaviour
    {
        public enum MoveMode { Once, Loop, PingPong }

        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _duration = 5f;
        [SerializeField] private MoveMode _mode = MoveMode.Loop;
        [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
        [SerializeField] private UnityEvent<float> _onPositionChanged;

        private float _t = 0f;
        private bool _movingForward = true;
        private float _totalLength = 0f;
        private Vector3 _cachedLocalScale = Vector3.one;

        private void Start()
        {
            CalculateTotalLength();
            _cachedLocalScale = transform.localScale;
            transform.position = _lineRenderer.GetPosition(0);
        }

        private void Update()
        {
            if (_totalLength == 0) return;

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
            transform.position = GetPositionAlongLine(_t * _totalLength) + _lineRenderer.transform.position;

            var scaledT = _scaleCurve.Evaluate(_t);
            transform.localScale = _cachedLocalScale * scaledT;
            _onPositionChanged?.Invoke(scaledT);
        }

        private void CalculateTotalLength()
        {
            _totalLength = 0f;
            for (var i = 0; i < _lineRenderer.positionCount - 1; i++)
            {
                _totalLength += Vector3.Distance(_lineRenderer.GetPosition(i), _lineRenderer.GetPosition(i + 1));
            }
        }

        private Vector3 GetPositionAlongLine(float distance)
        {
            Vector3 position;

            var lr = _lineRenderer;
            for (var i = 0; i < lr.positionCount - 1; i++)
            {
                var segmentLength = Vector3.Distance(lr.GetPosition(i), lr.GetPosition(i + 1));
                if (distance <= segmentLength)
                {
                    position = Vector3.Lerp(lr.GetPosition(i), lr.GetPosition(i + 1), distance / segmentLength);
                    position.x *= lr.transform.lossyScale.x;
                    position.y *= lr.transform.lossyScale.y;
                    position.z *= lr.transform.lossyScale.z;
                    return position;
                }
                distance -= segmentLength;
            }
            position = lr.GetPosition(lr.positionCount - 1);
            position.x *= lr.transform.lossyScale.x;
            position.y *= lr.transform.lossyScale.y;
            position.z *= lr.transform.lossyScale.z;
            return position;
        }
    }
}
