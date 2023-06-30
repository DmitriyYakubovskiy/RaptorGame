using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public static int m_indexScene;

    public void ChangeScenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes);
    }

    public void RestartScene()
    {
        m_indexScene = (SceneManager.GetActiveScene().buildIndex);
        ChangeScenes(0);
    }

    public void NextScene()
    {
        m_indexScene = (SceneManager.GetActiveScene().buildIndex)+1;
        ChangeScenes(0);
    }
}
