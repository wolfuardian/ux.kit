using System;
using UnityEngine;

namespace Eos.Ux.Kit
{
    public enum MoveMode { Once, Loop, PingPong }

    public class LsPathFollower : MonoBehaviour
    {
        public LineRenderer _lineRenderer;
        public float _duration = 5f;
        public MoveMode _mode = MoveMode.Loop;

        private float _distanceTraveled = 0f;
        private bool _movingForward = true;
        private float _totalLength = 0f;

        private void Start()
        {
            CalculateTotalLength();
            transform.position = _lineRenderer.GetPosition(0);
        }

        private void Update()
        {
            if (_totalLength == 0) return;

            var speed = 1f / _duration;
            _distanceTraveled += speed * Time.deltaTime * (_movingForward ? 1 : -1);

            switch (_mode)
            {
                case MoveMode.Once:
                    if (_distanceTraveled >= 1f)
                    {
                        _distanceTraveled = 1f;
                        enabled = false;
                    }
                    break;
                case MoveMode.Loop:
                    if (_distanceTraveled >= 1f) _distanceTraveled -= 1f;
                    else if (_distanceTraveled < 0) _distanceTraveled += 1f;
                    break;
                case MoveMode.PingPong:
                    if (_distanceTraveled is >= 1f or <= 0)
                    {
                        _movingForward = !_movingForward;
                        _distanceTraveled = Mathf.Clamp(_distanceTraveled, 0, 1f);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.position = GetPositionOnPath(_distanceTraveled * _totalLength) + _lineRenderer.transform.position;
        }

        private void CalculateTotalLength()
        {
            _totalLength = 0f;
            for (var i = 0; i < _lineRenderer.positionCount - 1; i++)
            {
                _totalLength += Vector3.Distance(_lineRenderer.GetPosition(i), _lineRenderer.GetPosition(i + 1));
            }
        }

        private Vector3 GetPositionOnPath(float distance)
        {
            var remaining = distance;
            for (var i = 0; i < _lineRenderer.positionCount - 1; i++)
            {
                var segmentLength = Vector3.Distance(_lineRenderer.GetPosition(i), _lineRenderer.GetPosition(i + 1));
                if (remaining <= segmentLength)
                {
                    return Vector3.Lerp(_lineRenderer.GetPosition(i), _lineRenderer.GetPosition(i + 1), remaining / segmentLength);
                }
                remaining -= segmentLength;
            }
            return _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
        }
    }
}
