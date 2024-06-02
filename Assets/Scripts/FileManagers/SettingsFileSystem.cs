using System.IO;
using UnityEngine;

public class SettingsData
{
    public const float kValueVolume = 20f;
    public float volumeValue= Mathf.Log10(1) * kValueVolume;
    public float musicValue= Mathf.Log10(1) * kValueVolume;
    public SystemLanguage language = SystemLanguage.Unknown;
    public bool fpsIsOn = false;
    public bool isZoomed = false;
}


public class SettingsFileManager : IFileManager
{
    private string pathLevelsData = "/settingsData.json";

    public void SaveData(object obj)
    {
        string json = JsonUtility.ToJson(obj as SettingsData);
        File.WriteAllText(Application.persistentDataPath + pathLevelsData, json);
    }

    public object LoadData()
    {
        SettingsData settings;
        string path = Application.persistentDataPath + pathLevelsData;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonUtility.FromJson<SettingsData>(json);
        }
        else
        {
            settings=new SettingsData();
        }

        return settings;
    }
}