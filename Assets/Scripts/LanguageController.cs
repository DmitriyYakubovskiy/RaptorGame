using OneClickLocalization;
using TMPro;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    SystemLanguage currentLanguage;

    private void Awake()
    {
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;

        if (data.language == SystemLanguage.Unknown) currentLanguage = OCL.GetLanguage();
        if (data.language == SystemLanguage.Russian)
        {
            OCL.SetLanguage(SystemLanguage.Russian);
            currentLanguage = OCL.GetLanguage();
            dropdown.value = 1;
        }
        else
        {
            OCL.SetLanguage(SystemLanguage.English);
            currentLanguage = OCL.GetLanguage();
            dropdown.value = 0;
        }
    }
    public void ChangeLanguage(int val)
    {
        if (val == 0)
        {
            OCL.SetLanguage(SystemLanguage.English);
            currentLanguage = SystemLanguage.English;
        }
        if (val == 1)
        {
            OCL.SetLanguage(SystemLanguage.Russian);
            currentLanguage = SystemLanguage.Russian;
        }
    }

    private void OnDestroy()
    {
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        data.language = currentLanguage;
        manager.SaveData(data);
    }
}
