using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NADI.Eos
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public class UxMeshButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onClick = new UnityEvent();
        [SerializeField] private UnityEvent _onPointerEnter = new UnityEvent();
        [SerializeField] private UnityEvent _onPointerExit = new UnityEvent();
        [SerializeField] private UnityEvent _onPointerDown = new UnityEvent();
        [SerializeField] private UnityEvent _onPointerUp = new UnityEvent();

        private Camera _mainCamera;
        private Collider _cachedCollider;

        private bool _isPressed;
        private bool _isHovered;

        private Vector3 _mousePosition;
        private Vector3 _pressStartPosition;

        private bool _mouseButtonDown;
        private bool _mouseButtonUp;

        private readonly float _clickThreshold = 10f;

        [SerializeField] private bool m_HasSelected = false;

        private Camera _camera;
        private Camera cachedCamera => _camera ??= Camera.main;

        private void Awake()
        {
            _cachedCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            _mousePosition = Input.mousePosition;
            _mouseButtonDown = Input.GetMouseButtonDown(0);
            _mouseButtonUp = Input.GetMouseButtonUp(0);
        }

        private void LateUpdate()
        {
            if (cachedCamera == null || _cachedCollider == null)
                return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                if (_isHovered)
                {
                    _isHovered = false;
                    _onPointerExit.Invoke();
                }
                return;
            }
            ProcessIndividualInput();
        }

        private void ProcessIndividualInput()
        {
            var isHovering = Physics.Raycast(cachedCamera.ScreenPointToRay(_mousePosition), out var hit)
                && hit.collider == _cachedCollider;

            if (_mouseButtonDown && isHovering)
            {
                _isPressed = true;
                _onPointerDown.Invoke();
                _pressStartPosition = _mousePosition;
            }
            else if (_mouseButtonUp)
            {
                if (_isPressed)
                {
                    var movedDistance = Vector2.Distance(_mousePosition, _pressStartPosition);
                    if (movedDistance <= _clickThreshold && isHovering)
                    {
                        _onClick.Invoke();
                    }
                }
                _isPressed = false;
                _onPointerUp.Invoke();
            }

            if (isHovering)
            {
                if (!_isHovered)
                {
                    _isHovered = true;
                    _onPointerEnter.Invoke();
                }
            }
            else if (_isHovered)
            {
                _isHovered = false;
                _onPointerExit.Invoke();
            }
        }
    }
}
