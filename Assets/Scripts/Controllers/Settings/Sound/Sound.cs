using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public SoundArray[] sounds;
    private AudioSource audioSrc => GetComponent<AudioSource>();
    [SerializeField] protected float volume = 0.2f;

    private void Awake()
    {
        audioSrc.outputAudioMixerGroup = mixerGroup;
    }

    public void PlaySound()
    {
        if (sounds.Length == 0) return;
        AudioClip clip = sounds[0].soundArray[Random.Range(0, sounds[0].soundArray.Length)];
        audioSrc.pitch = Random.Range(0.85f, 1.2f);
        audioSrc.PlayOneShot(clip, volume);
    }

    public void PlaySound(int i, float volume = 1f, float p1 = 0.85f, float p2 = 1.2f, bool isDestroyed = false)
    {
        if (sounds.Length <= i) return;
        if (mixerGroup.name == "None") Debug.Log(gameObject.name);
        int index = Random.Range(0, sounds[i].soundArray.Length);
        AudioClip clip = sounds[i].soundArray[index];
        audioSrc.pitch = Random.Range(p1, p2);
        if (isDestroyed)
        {
            GameObject soundObj = new GameObject("Sound");
            soundObj.transform.position = transform.position;
            Instantiate(soundObj);
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.spatialBlend = 1f;
            source.volume = volume;
            source.Play();
            Destroy(soundObj, clip.length);
            //AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
        else
        {
            audioSrc.PlayOneShot(clip, volume);
        }
    }

    public void AudioStop()
    {
        if (audioSrc.isPlaying) audioSrc.Stop();
    }

    public void AudioPause()
    {
        if (audioSrc.isPlaying) audioSrc.Pause();
    }

    public void AudioStart(int index = -1, float v = 0.2f)
    {
        if (!audioSrc.isPlaying)
        {
            if (index == -1) audioSrc.UnPause();
            else PlaySound(index, volume = v);
        }
    }

    [System.Serializable]
    public class SoundArray
    {
        public AudioClip[] soundArray;
    }
}

