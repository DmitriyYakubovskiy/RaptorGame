using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool m_fpsIsEnabled;
    [SerializeField] private Toggle m_boxFps;
    [SerializeField] private GameObject m_fpsText;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("ShowFps") == 1)
        {
            m_boxFps.isOn = true;
        }
        else
        {
            m_boxFps.isOn = false;
        } 
    }

    public void ShowFps()
    {
        if(m_boxFps.isOn)
        {
            PlayerPrefs.SetInt("ShowFps",1);
            m_fpsText.SetActive(true);
        }
        else 
        {
            PlayerPrefs.SetInt("ShowFps", 0);
            m_fpsText.SetActive(false);
        }
    }


}
