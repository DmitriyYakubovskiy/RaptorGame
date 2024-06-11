using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    private void Start()
    {
        for (int i = 1; i < SaveManager.Data.levels.openLevels.Length; i++)
        {
            if (i >= buttons.Length) return;
            if (SaveManager.Data.levels.openLevels[i]) buttons[i].interactable = true;
            else buttons[i].interactable = false;
        }
    }

    public void LevelIsAvailable(int numberScene)
    {
        if (SaveManager.Data.levels.openLevels[numberScene-1] == true)
        {
            SceneManager.ChangeScene(numberScene);
        }
    }   
}
