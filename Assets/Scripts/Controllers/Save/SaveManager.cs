using Assets.Scripts.Controllers.Save;
using System.Collections;
using UnityEngine;
using YG;

[System.Serializable]
public class LevelsData
{
    public bool[] openLevels = new bool[15];
}

[System.Serializable]
public class RaptorData
{
    public int level;
    public int upgradePoints;
    public int knockback;
    public float experience;
    public float lives;
    public float speed;
    public float attack;
}

[System.Serializable]
public class SettingsData
{
    public string gameLanguage;
    public float kValueVolume;
    public float volumeValue ;
    public float musicValue;
    public bool fpsIsOn;
    public bool isZoomed;
}

[System.Serializable]
public class SaveData
{
    public SettingsData settings;
    public RaptorData raptor;
    public LevelsData levels;

    public SaveData()
    {
        settings = new SettingsData();
        raptor=new RaptorData();
        levels=new LevelsData();
    }

    public static explicit operator SaveData(SavesYG dataYG)
    {
        SaveData data=new SaveData();
        data.settings.volumeValue = dataYG.volumeValue;
        data.settings.musicValue = dataYG.musicValue;
        data.settings.isZoomed = dataYG.isZoomed;
        data.settings.fpsIsOn = dataYG.fpsIsOn;
        data.settings.gameLanguage=dataYG.gameLanguage;
        data.settings.kValueVolume = SavesYG.kValueVolume;

        data.raptor.level=dataYG.level;
        data.raptor.upgradePoints=dataYG.upgradePoints;
        data.raptor.experience=dataYG.experience;
        data.raptor.lives=dataYG.lives;
        data.raptor.speed=dataYG.speed;
        data.raptor.attack=dataYG.attack;
        data.raptor.knockback=dataYG.knockback;

        for (int i = 0; i < data.levels.openLevels.Length; i++)
        {
            data.levels.openLevels[i] = dataYG.openLevels[i];
        }

        return data;
    }
}

public class SaveManager : MonoBehaviour
{
    private const float AutoSaveInterval = 60f;
    private ISaveService service;

    public static SaveData Data;

    private void Awake()
    {
        Data = new SaveData();
        service = new YGSaveService();
        if (YandexGame.SDKEnabled == true)
        {
            Load();
            StartCoroutine("AutoSave");
        }
    }

    public void Save()
    {
        service.Save(Data);
    }

    public void ResetProgress()
    {
        var emptyData = (SaveData)(new SavesYG());
        service.Save(emptyData);
        Load();
        SceneManager.RestartScene();
    }

    private void Load()
    {
        Data = service.Load();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            Save();
            yield return new WaitForSeconds(AutoSaveInterval);
        }
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    private void OnDestroy()
    {
        Save();
    }

}
