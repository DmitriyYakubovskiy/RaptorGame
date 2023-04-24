using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public static bool[] m_levels = new bool[3];
    [SerializeField] private Button[] m_buttons;
    [SerializeField] private Scenes m_scene;

    private void Awake()
    {
        m_levels[0] = true;
        for (int i = 1; i < m_levels.Length; i++)
        {
            if (PlayerPrefs.GetInt("level"+i.ToString()) == 1)
            {
                m_levels[i] = true;
            }
            else
            {
                m_levels[i] = false;
            }
        }
    }

    private void Start()
    {
        for (int i = 1; i < m_levels.Length; i++)
        {
            if (m_levels[i])
            {
                m_buttons[i].interactable = true; 
            }
            else
            {
                m_buttons[i].interactable = false;
            }
        }
    }

    public void LevelIsAvailable(int numberScene)
    {
        if (m_levels[numberScene - 1] == true)
        {
            m_scene.ChangeScenes(numberScene);
        }
    }   
}
