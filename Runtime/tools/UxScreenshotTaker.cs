#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

#if UNITY_EDITOR
namespace Ux.Kit
{
    public static class UxScreenshotTaker
    {
        [MenuItem("Ux Kit/Utils/TakeScreenshots")]
        public static void MenuItem_TakeScreenshots()
        {
            var gameViewType = System.Type.GetType("UnityEditor.GameView, UnityEditor");
            var gameView     = EditorWindow.GetWindow(gameViewType);
            if (gameView == null)
                EditorApplication.ExecuteMenuItem("Waindow/Game");

            gameView = EditorWindow.GetWindow(gameViewType);
            if (gameView == null)
            {
                Debug.LogError("Game View not find!");
                return;
            }

            const int WIDTH                = 1024;
            const int HEIGHT               = 768;
            const int GAME_VIEW_BAR_HEIGHT = 17;

            var rect = gameView.position;
            rect.x = 0;
            rect.y = 0;
            rect.width = WIDTH;
            rect.height = HEIGHT + GAME_VIEW_BAR_HEIGHT;
            gameView.position = rect;

            var fileName   = "screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";
            var folderPath = Application.persistentDataPath;
            fileName = System.IO.Path.Combine(folderPath, fileName);
            ScreenCapture.CaptureScreenshot(fileName);
            Application.OpenURL(folderPath);
        }
    }
}
#endif
