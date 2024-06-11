using Assets.Scripts.Controllers.Save;
using YG;

public class YGSaveService : ISaveService
{
    public void Save(SaveData data)
    {
        YandexGame.savesData = (SavesYG)data;
        YandexGame.SaveProgress();
    }

    public SaveData Load()
    {
        return (SaveData)YandexGame.savesData;
    }
}

