using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle boxFps;
    [SerializeField] private GameObject fpsText;

    public bool m_fpsIsEnabled;

    private void Awake()
    {
        boxFps.isOn = SaveManager.Data.settings.fpsIsOn;
    }

    public void ShowFps()
    {
        if (boxFps.isOn)
        {
            SaveManager.Data.settings.fpsIsOn = true;
            fpsText.SetActive(true);
        }
        else 
        {
            SaveManager.Data.settings.fpsIsOn = false;
            fpsText.SetActive(false);
        }
    }


}
