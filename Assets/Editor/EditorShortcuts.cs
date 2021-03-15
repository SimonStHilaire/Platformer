using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorShortcuts
{
    [MenuItem("Shortcuts/Go To Main Scene %F1")]
    public static void GoToMainScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/main.unity");
    }
}
