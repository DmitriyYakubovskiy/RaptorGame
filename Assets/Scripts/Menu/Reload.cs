using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    private void Start()
    {
        if (Scenes.m_indexScene != 0)
        {
            SceneManager.LoadScene(Scenes.m_indexScene);
            Scenes.m_indexScene = 0;
        }
    }
}
