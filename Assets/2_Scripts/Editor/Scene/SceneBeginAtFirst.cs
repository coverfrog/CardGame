#if UNITY_EDITOR && true
using UnityEditor;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneBeginAtFirst
{
    static SceneBeginAtFirst()
    {
        EditorApplication.playModeStateChanged -= ChangePlayMode;
        EditorApplication.playModeStateChanged += ChangePlayMode;
    }

    private static void ChangePlayMode(PlayModeStateChange state)
    {
        if (state is not PlayModeStateChange.EnteredPlayMode)
        {
            return;
        }

        SceneManager.LoadScene(0);
    }
}

#endif