#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class BlenderViewShortcuts
{
    static Vector3 lastDirection = Vector3.zero;
    static Vector3 lastUp = Vector3.up;

    static BlenderViewShortcuts()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type != EventType.KeyDown) return;

        void StoreCurrentView()
        {
            lastDirection = sceneView.camera.transform.forward;
            lastUp = sceneView.camera.transform.up;
        }

        switch (e.keyCode)
        {
            case KeyCode.Keypad1: // Front view (Z+)
                StoreCurrentView();
                SetSceneViewDirection(Vector3.back, Vector3.up); // Looking from Z+ toward origin
                e.Use();
                break;

            case KeyCode.Keypad3: // Right view (X+)
                StoreCurrentView();
                SetSceneViewDirection(Vector3.left, Vector3.up); // Looking from X+ toward origin
                e.Use();
                break;

            case KeyCode.Keypad7: // Top view (Y+)
                StoreCurrentView();
                SetSceneViewDirection(Vector3.down, Vector3.forward); // Looking from Y+ downward
                e.Use();
                break;

            case KeyCode.Keypad9: // Opposite view
                StoreCurrentView();
                Vector3 currentDir = sceneView.camera.transform.forward;
                Vector3 currentUp = sceneView.camera.transform.up;
                SetSceneViewDirection(-currentDir, currentUp);
                e.Use();
                break;

            case KeyCode.Keypad5: // Last view
                if (lastDirection != Vector3.zero)
                {
                    SetSceneViewDirection(lastDirection, lastUp);
                    e.Use();
                }
                break;

            case KeyCode.Keypad4: // Toggle Orthographic
                sceneView.orthographic = !sceneView.orthographic;
                sceneView.Repaint();
                e.Use();
                break;
        }
    }

    static void SetSceneViewDirection(Vector3 direction, Vector3 up)
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null) return;

        sceneView.LookAt(sceneView.pivot, Quaternion.LookRotation(direction, up), sceneView.size);
        sceneView.Repaint();
    }
}
#endif
