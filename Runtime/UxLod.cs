using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX Kit/UX LOD")]
    public class UxLod : MonoBehaviour
    {
        [SerializeField] private List<LodSettings> _lodSettings;
        [SerializeField] private float _currentFov;

        private Camera _camera;
        private Camera cachedCamera => _camera ??= Camera.main;

        private void Start()
        {
            _lodSettings.Sort((a, b) => a._zoom.CompareTo(b._zoom));
        }

        private void Update()
        {
            _currentFov = cachedCamera.fieldOfView;
        }

        private void LateUpdate()
        {
            foreach (var settings in _lodSettings.Where(settings => settings._zoom <= _currentFov))
            {
                transform.localScale = Vector3.one * settings._scale;
            }
        }

        [Serializable]
        private class LodSettings
        {
            [Header("If Greater Than")]
            public float _zoom;
            public float _scale;
        }
    }
}
