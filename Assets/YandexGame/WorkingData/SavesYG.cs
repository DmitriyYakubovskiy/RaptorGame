
using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int money = 1;                      
        public string newPlayerName = "Hello!";
        //public bool[] openLevels = new bool[3];

        //Raptor
        public int level = 1;
        public int upgradePoints = 0;
        public float experience = 0;
        public float lives = 50;
        public float speed = 8;
        public float attack = 5;
        public int knockback = 15;
        //Settings
        public string gameLanguage = "Russian";
        public const float kValueVolume = 20f;
        public float volumeValue = Mathf.Log10(1) * kValueVolume;
        public float musicValue = Mathf.Log10(1) * kValueVolume;
        public bool fpsIsOn = false;
        public bool isZoomed = false;
        //Levels
        public bool[] openLevels =new bool[15];

        public SavesYG()
        {
            openLevels[0] = true;
        }

        public static explicit operator SavesYG(SaveData data)
        {
            SavesYG dataYG = new SavesYG();
            dataYG.volumeValue = data.settings.volumeValue;
            dataYG.musicValue = data.settings.musicValue;
            dataYG.isZoomed = data.settings.isZoomed;
            dataYG.fpsIsOn = data.settings.fpsIsOn;
            dataYG.gameLanguage = data.settings.gameLanguage;

            dataYG.level = data.raptor.level;
            dataYG.upgradePoints = data.raptor.upgradePoints;
            dataYG.experience = data.raptor.experience;
            dataYG.lives = data.raptor.lives;
            dataYG.speed = data.raptor.speed;
            dataYG.attack = data.raptor.attack;
            dataYG.knockback = data.raptor.knockback;

            for(int i = 0; i < dataYG.openLevels.Length; i++)
            {
                dataYG.openLevels[i] = data.levels.openLevels[i];
            }
            
            return dataYG;
        }
    }
}
