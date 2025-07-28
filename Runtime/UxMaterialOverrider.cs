using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Ux.Kit
{
    public class UxMaterialOverrider : MonoBehaviour
    {
        [SerializeField] private Material _materialTarget;

        [SerializeField] private Settings _settings = new Settings();

        private readonly List<MeshRendererData> _meshRendererData = new List<MeshRendererData>();
        private readonly List<LineRendererData> _lineRendererData = new List<LineRendererData>();
        private readonly List<ImageRendererData> _imageData = new List<ImageRendererData>();

        private bool HasAnyMeshRenderer
        {
            get
            {
                return _meshRendererData != null && _meshRendererData.Count > 0;
            }
        }
        private bool HasAnyLineRenderer
        {
            get
            {
                return _lineRendererData != null && _lineRendererData.Count > 0;
            }
        }
        private bool HasAnyImage
        {
            get
            {
                return _imageData != null && _imageData.Count > 0;
            }
        }

        public void OverrideMaterials(bool isOverride)
        {
            _meshRendererData.ForEach(data => data._meshRenderer.sharedMaterials = isOverride ? data._materialsInstance : data._materialsDefault);
            _lineRendererData.ForEach(data => data._lineRenderer.sharedMaterials = isOverride ? data._materialsInstance : data._materialsDefault);
            _imageData.ForEach(data => data._image.material = isOverride ? data._materialInstance : data._materialDefault);
        }

        public void OverrideMaterials() => OverrideMaterials(isOverride: true);
        public void RestoreDefaultMaterial() => OverrideMaterials(isOverride: false);

        private void Awake()
        {
            _meshRendererData.Clear();
            _lineRendererData.Clear();
            _imageData.Clear();

            var mrs = _settings._overrideCustomRenderers ? _settings._meshRenderers : GetComponentsInChildren<MeshRenderer>().ToList();
            mrs.ForEach(meshRenderer =>
            {
                _meshRendererData.Add(new MeshRendererData
                {
                    _meshRenderer = meshRenderer,
                    _materialsDefault = meshRenderer.sharedMaterials,
                    _materialsInstance = meshRenderer.sharedMaterials
                });
            });
            var mls = _settings._overrideCustomRenderers ? _settings._lineRenderers : GetComponentsInChildren<LineRenderer>().ToList();
            mls.ForEach(lineRenderer =>
            {
                _lineRendererData.Add(new LineRendererData
                {
                    _lineRenderer = lineRenderer,
                    _materialsDefault = lineRenderer.sharedMaterials,
                    _materialsInstance = lineRenderer.sharedMaterials
                });
            });
            var imageRenderers = _settings._overrideCustomRenderers ? _settings._images : GetComponentsInChildren<UnityEngine.UI.Image>().ToList();
            imageRenderers.ForEach(imageRenderer =>
            {
                _imageData.Add(new ImageRendererData
                {
                    _image = imageRenderer,
                    _materialDefault = imageRenderer.material,
                    _materialInstance = imageRenderer.material
                });
            });

            _meshRendererData.ForEach(data => System.Array.Fill(data._materialsInstance, _materialTarget));
            _lineRendererData.ForEach(data => System.Array.Fill(data._materialsInstance, _materialTarget));
            _imageData.ForEach(data => data._materialInstance = _materialTarget);

            if (!HasAnyMeshRenderer && !HasAnyLineRenderer && !HasAnyImage)
            {
                Debug.LogWarning("AfMaterialOverride: No MeshRenderer ,LineRenderer or Image found in children. Please add at least one to use material override functionality.", this);
            }
        }

        private void Start()
        {
            if (_settings._overrideOnStart)
            {
                OverrideMaterials();
            }
        }

        [System.Serializable]
        public class Settings
        {
            public bool _overrideOnStart = false;
            public bool _overrideCustomRenderers = false;
            public List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
            public List<LineRenderer> _lineRenderers = new List<LineRenderer>();
            public List<UnityEngine.UI.Image> _images = new List<UnityEngine.UI.Image>();
        }

        [System.Serializable]
        public class MeshRendererData
        {
            public MeshRenderer _meshRenderer;
            public Material[] _materialsDefault;
            public Material[] _materialsInstance;
        }

        [System.Serializable]
        public class LineRendererData
        {
            public LineRenderer _lineRenderer;
            public Material[] _materialsDefault;
            public Material[] _materialsInstance;
        }

        [System.Serializable]
        public class ImageRendererData
        {
            public UnityEngine.UI.Image _image;
            public Material _materialDefault;
            public Material _materialInstance;
        }
    }
}
