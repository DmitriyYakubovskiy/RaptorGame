using System.IO;
using UnityEngine;

public class LevelData
{
    public static string pathLevelsData = "/levelsData.json";
    public bool[] levels = new bool[4];

    public static LevelData Convert(bool[] levels)
    {
        LevelData levelData = new LevelData();
        levelData.levels[0] = levels[0];
        for (int i = 1; i < levels.Length; i++)
        {
            levelData.levels[i] = levels[i];
        }
        return levelData;
    }
}


public class LevelsFileManager :IFileManager
{
    public void SaveData(object obj)
    {
        string json = JsonUtility.ToJson(obj as LevelData);
        File.WriteAllText(Application.persistentDataPath + LevelData.pathLevelsData, json);
    }

    public object LoadData()
    {
        LevelData level;
        string path = Application.persistentDataPath + LevelData.pathLevelsData;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            level = JsonUtility.FromJson<LevelData>(json);
        }
        else
        {
            level = new LevelData();
            level.levels[0] = true;
            for (int i = 1; i < level.levels.Length; i++)
            {
                level.levels[i] = false;
            }
            SaveData(level);
        }

        return level;
    }
}
