using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static int m_indexScene;

    public static void ChangeScene(int numberScenes)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(numberScenes);
    }

    public static void RestartScene()
    {
        m_indexScene = (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        ChangeScene(m_indexScene);
    }

    public static void NextScene()
    {
        m_indexScene = (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)+1;
        ChangeScene(m_indexScene+1);
    }

    public static void EndlessModeScene()
    {
        ChangeScene(UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings-1);
    }
}
