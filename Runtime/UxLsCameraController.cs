using UnityEngine;

namespace Ux.Kit
{
    [AddComponentMenu("UX/Kit/UX Ls Camera Controller")]
    public class UxLsCameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public void SetAllLayers()
        {
            UxCameraHelper.IncludeAllLayers(_camera);
        }

        public void SetAllLayersExcept(string layerName)
        {
            UxCameraHelper.ExcludeLayer(_camera, layerName);
        }

        public void SetDefaultLayers()
        {
            UxCameraHelper.IncludeDefaultLayers(_camera);
        }

        public void SetLayer(string layerName)
        {
            UxCameraHelper.IncludeLayer(_camera, layerName);
        }
    }
}
