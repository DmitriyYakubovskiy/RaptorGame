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
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        m_boxFps.isOn = data.fpsIsOn;
    }

    public void ShowFps()
    {
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        if (m_boxFps.isOn)
        {
            data.fpsIsOn=true;
            manager.SaveData(data);
            m_fpsText.SetActive(true);
        }
        else 
        {
            data.fpsIsOn = false;
            manager.SaveData(data);
            m_fpsText.SetActive(false);
        }
    }


}
