#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

public static class InspectorLockToggle
{
    [MenuItem("Tools/Toggle Active Window Lock &1")] // Ctrl + Alt + L
    public static void ToggleActiveWindowLock()
    {
        // Get the currently focused editor window
        EditorWindow window = EditorWindow.focusedWindow;
        if (window == null)
        {
            Debug.Log("No window is focused.");
            return;
        }

        // Get the type of the focused window
        var type = window.GetType();

        // Look for the 'isLocked' property (works with Inspector and similar windows)
        PropertyInfo isLockedProp = type.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (isLockedProp == null)
        {
            Debug.Log("This window does not support locking.");
            return;
        }

        // Toggle the lock state
        bool isLocked = (bool)isLockedProp.GetValue(window);
        isLockedProp.SetValue(window, !isLocked);
        window.Repaint();

        Debug.Log($"{type.Name} lock toggled to {!isLocked}");
    }
}
#endif
