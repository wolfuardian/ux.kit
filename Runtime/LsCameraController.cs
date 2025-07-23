using UnityEngine;

namespace Eos.Ux.Kit
{
    public class LsCameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public void SetAllLayers()
        {
            LsCameraHelper.IncludeAllLayers(_camera);
        }

        public void SetAllLayersExcept(string layerName)
        {
            LsCameraHelper.ExcludeLayer(_camera, layerName);
        }

        public void SetDefaultLayers()
        {
            LsCameraHelper.IncludeDefaultLayers(_camera);
        }

        public void SetLayer(string layerName)
        {
            LsCameraHelper.IncludeLayer(_camera, layerName);
        }
    }
}
