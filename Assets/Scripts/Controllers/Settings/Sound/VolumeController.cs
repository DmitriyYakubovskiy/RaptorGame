using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private string volueParameter = "MasterVol";
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void Start()
    { 
        if (volueParameter == "MusicVol") slider.value = Mathf.Pow(10f, SaveManager.Data.settings.musicValue / SaveManager.Data.settings.kValueVolume);
        else slider.value = Mathf.Pow(10f, SaveManager.Data.settings.volumeValue / SaveManager.Data.settings.kValueVolume);
    }

    private void HandleSliderValueChanged(float value)
    {
        if (volueParameter == "MusicVol")
        {
            SaveManager.Data.settings.musicValue = Mathf.Log10(value) * SaveManager.Data.settings.kValueVolume;
            mixer.SetFloat(volueParameter, SaveManager.Data.settings.musicValue);
        }
        else
        {
            SaveManager.Data.settings.volumeValue = Mathf.Log10(value) * SaveManager.Data.settings.kValueVolume;
            mixer.SetFloat(volueParameter, SaveManager.Data.settings.volumeValue);
        }
    }
}
