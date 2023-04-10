using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void ChangeScanes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes);
    }
}
