using UnityEditor;
using UnityEngine;
using Ux.Kit;

namespace Eos.Ux.Kit.Editor
{
    public static class UxKitMenu
    {
        [MenuItem("GameObject/Eos/UX Kit/DrawCircle", false, 3)]
        public static void CreateLeanPointOfViewHierarchy()
        {
            var drawGo = new GameObject("new DrawCircle");

            SetupDrawCircle(drawGo);

            Undo.RegisterCreatedObjectUndo(drawGo, "Create DrawCircle");

            drawGo.transform.SetParent(Selection.activeTransform, false);

            Selection.activeGameObject = drawGo;
        }

        private static void SetupDrawCircle(GameObject drawGo)
        {
            const string BASE_NAME = "DrawCircle";

            var uxDrawCircle = drawGo.AddComponent<UxDrawCircle>();
            var lineRenderer = drawGo.GetComponent<LineRenderer>();
            lineRenderer.loop = true;
        }
    }
}
