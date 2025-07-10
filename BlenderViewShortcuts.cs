#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class BlenderViewShortcuts
{
    static Vector3 view0Direction = Vector3.zero;
    static Vector3 view0Up = Vector3.up;
    static bool view0Ortho = false;
    static bool hasSavedView0 = false;
    static Vector3 view0Pivot;
    static float view0Size;

    static BlenderViewShortcuts()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type != EventType.KeyDown) return;

        void SaveView0()
        {
            if (!hasSavedView0)
            {
                view0Direction = sceneView.camera.transform.forward;
                view0Up = sceneView.camera.transform.up;
                view0Ortho = sceneView.orthographic;
                view0Pivot = sceneView.pivot;
                view0Size = sceneView.size;
                hasSavedView0 = true;
            }
        }

        //void FocusOnSelection(SceneView sv)
        //{
        //    if (Selection.activeTransform != null)
        //    {
        //       sv.pivot = Selection.activeTransform.position;
        //      sv.size = 5f; // Adjust zoom level if needed
        //    }
        //}

        switch (e.keyCode)
        {
            case KeyCode.Keypad1: // Front (Z+)
                SaveView0();
                SetSceneViewDirection(Vector3.back, Vector3.up, focus: true);
                e.Use();
                break;

            case KeyCode.Keypad3: // Right (X+)
                SaveView0();
                SetSceneViewDirection(Vector3.left, Vector3.up, focus: true);
                e.Use();
                break;

            case KeyCode.Keypad7: // Top (Y+)
                SaveView0();
                SetSceneViewDirection(Vector3.down, Vector3.forward, focus: true);
                e.Use();
                break;

            case KeyCode.Keypad9: // Opposite of current view
                SaveView0();
                Vector3 currentDir = sceneView.camera.transform.forward;
                Vector3 currentUp = sceneView.camera.transform.up;
                SetSceneViewDirection(-currentDir, currentUp, focus: true);
                e.Use();
                break;

            case KeyCode.Keypad5: // Return to view 0
                if (hasSavedView0)
                {
                    SceneView sv = SceneView.lastActiveSceneView;
                    if (sv != null)
                    {
                        sv.orthographic = view0Ortho;
                        sv.LookAt(view0Pivot, Quaternion.LookRotation(view0Direction, view0Up), view0Size);
                        sv.Repaint();
                        hasSavedView0 = false;
                        e.Use();
                    }
                }
                break;

            case KeyCode.Keypad4: // Toggle Ortho
                sceneView.orthographic = !sceneView.orthographic;
                sceneView.Repaint();
                e.Use();
                break;
        }
    }

    static void SetSceneViewDirection(Vector3 direction, Vector3 up, bool focus)
    {
        SceneView sv = SceneView.lastActiveSceneView;
        if (sv == null) return;

        if (focus && Selection.activeTransform != null)
        {
            sv.pivot = Selection.activeTransform.position;
            sv.size = 5f; // You can adjust zoom level here
        }

        sv.LookAt(sv.pivot, Quaternion.LookRotation(direction, up), sv.size);
        sv.Repaint();
    }
}
#endif
