using System.Linq;
using UnityEngine;

namespace Eos.Ux.Kit
{
    public static class LsCameraHelper
    {
        private static readonly string[] DefaultLayers =
        {
            "Default",
            "TransparentFX",
            "Ignore Raycast",
            "Water",
            "UI"
        };

        public static void IncludeAllLayers(Camera cam)
        {
            cam.cullingMask = ~0;
        }

        public static void IncludeDefaultLayers(Camera cam)
        {
            cam.cullingMask = DefaultLayers.Select(LayerMask.NameToLayer).Aggregate(0, (current, layer) => current | 1 << layer);
        }

        public static void ExcludeLayer(Camera cam, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            cam.cullingMask = ~ (1 << layer);
        }

        public static void IncludeLayer(Camera cam, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            cam.cullingMask = DefaultLayers.Select(LayerMask.NameToLayer).Aggregate(0, (current, l) => current | 1 << l) | 1 << layer;
        }

        public static void IncludeAllExcept(Camera cam, params string[] excludedLayers)
        {
            var mask = excludedLayers.Select(LayerMask.NameToLayer).Aggregate(~0, (current, layer) => current & ~(1 << layer));
            cam.cullingMask = mask;
        }

        public static bool IsOccluded(Camera cam, Vector3 position, LayerMask? occlusionLayer = null)
        {
            var origin    = cam.transform.position;
            var direction = position - origin;
            var distance  = direction.magnitude;

            var layerToUse = occlusionLayer?.value ?? LayerMask.GetMask("Default");

            if (Physics.Raycast(origin, direction.normalized, out var hit, distance, layerToUse))
            {
                return hit.transform.position != position;
            }

            return false;
        }

        public static Vector2 WorldToScreenPoint(Camera camera, Vector3 targetPos)
        {
            return camera.WorldToScreenPoint(targetPos);
        }

        public static Vector2 ClipWorldToScreenPoint(Camera camera, Vector3 targetPos)
        {
            const float CLAMP_MARGIN = 0.01f;

            var viewportPoint = camera.WorldToViewportPoint(targetPos);

            // If the point is within the screen and far in front of the camera
            if (viewportPoint.z > 0.01f && viewportPoint.x >= CLAMP_MARGIN && viewportPoint.x <= 1 - CLAMP_MARGIN && viewportPoint.y >= CLAMP_MARGIN && viewportPoint.y <= 1 - CLAMP_MARGIN)
            {
                return camera.ViewportToScreenPoint(viewportPoint);
            }

            // If the point is outside the screen, constrain x and y to [0, 1] with a margin
            viewportPoint.x = Mathf.Clamp(viewportPoint.x, CLAMP_MARGIN, 1 - CLAMP_MARGIN);
            viewportPoint.y = Mathf.Clamp(viewportPoint.y, CLAMP_MARGIN, 1 - CLAMP_MARGIN);

            return camera.ViewportToScreenPoint(viewportPoint);
        }
    }
}
