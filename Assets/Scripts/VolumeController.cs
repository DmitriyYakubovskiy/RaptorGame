using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private string volueParameter = "MasterVol";
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;

    private float volumeValue;
    private const float kValueVolume = 20f;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value)
    {
        volumeValue = Mathf.Log10(value) * kValueVolume;
        mixer.SetFloat(volueParameter, volumeValue);
        Save();
    }

    private void Start()
    {
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        if (volueParameter == "MusicVol") volumeValue = data.musicValue;
        else volumeValue = data.volumeValue;
        slider.value = Mathf.Pow(10f, volumeValue/kValueVolume);
    }

    private void Save()
    {
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        if (volueParameter == "MusicVol")
        {
            data.musicValue = volumeValue;
            manager.SaveData(data);
        }
        else
        {
            data.volumeValue = volumeValue; ;
            manager.SaveData(data);
        }
    }

    private void OnDisable()
    {
        Save();
    }

    private void OnDestroy()
    {
        Save();
    }
}
