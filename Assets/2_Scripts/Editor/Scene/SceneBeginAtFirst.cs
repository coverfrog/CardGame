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
        
        // [25.08.03][cskim]
        // - 첫 번째 씬으로 로딩이 이미 되어 있다면 실행시키지 않음
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        
        SceneManager.LoadScene(0);
    }
}

#endif