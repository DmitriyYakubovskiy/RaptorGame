using OneClickLocalization;
using TMPro;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        if (SaveManager.Data.settings.gameLanguage == "Russian")
        {
            OCL.SetLanguage(SystemLanguage.Russian);
            dropdown.value = 1;
        }
        else
        {
            OCL.SetLanguage(SystemLanguage.English);
            dropdown.value = 0;
        }
    }
    public void ChangeLanguage(int val)
    {
        if (val == 0)
        {
            OCL.SetLanguage(SystemLanguage.English);
            SaveManager.Data.settings.gameLanguage = "English";
        }
        if (val == 1)
        {
            OCL.SetLanguage(SystemLanguage.Russian);
            SaveManager.Data.settings.gameLanguage = "Russian";
        }
    }
}
