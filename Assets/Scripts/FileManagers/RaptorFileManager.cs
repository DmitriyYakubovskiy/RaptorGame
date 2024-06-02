using UnityEngine;
using System.IO;

public class RaptorData
{
    public int level;
    public int upgradePoints;
    public float experience;
    public float lives;
    public float speed;
    public float attack;
    public int knockback;
}


public class RaptorFileManager : IFileManager
{
    private string pathRaptorData = "/raptorData.json";

    public void SaveData(object obj)
    {
        string json = JsonUtility.ToJson(obj as RaptorData);
        File.WriteAllText(Application.persistentDataPath + pathRaptorData, json);
    }

    public object LoadData()
    {
        RaptorData raptor;
        string path = Application.persistentDataPath + pathRaptorData;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            raptor = JsonUtility.FromJson<RaptorData>(json);
        }
        else
        {
            raptor = new RaptorData();
            raptor.level = 1;
            raptor.upgradePoints = 0;
            raptor.experience = 0;  
            raptor.lives = 50;
            raptor.speed = 8;
            raptor.attack = 5;
            raptor.knockback = 15;
            SaveData(raptor);
        }

        return raptor;
    }
}
