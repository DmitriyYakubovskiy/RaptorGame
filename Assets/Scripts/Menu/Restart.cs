using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private int m_indexScene;

    void Start()
    {
        m_indexScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(m_indexScene);
    }
}
