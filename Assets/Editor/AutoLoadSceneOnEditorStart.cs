#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoLoadSceneOnEditorStart
{
    static AutoLoadSceneOnEditorStart()
    {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        }
    }
}
#endif

