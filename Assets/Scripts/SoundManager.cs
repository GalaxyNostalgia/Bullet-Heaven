using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    private AudioSource _audioSource;
    public static SoundManager Instance
    {
        get
        {
            if (_instance ==null)
            {
                Debug.Log("SoundManager not found");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
           Destroy(gameObject);
           return;
        }

        _instance = this;
        _audioSource = GetComponent<AudioSource>();
        _instance.AdjustSoundVolume(0.25f);
        DontDestroyOnLoad(gameObject);
    }

    public float GetCurrentVolume()
    {
        return _audioSource.volume;
    }

    public bool IsMuted()
    {
        return _audioSource.mute;
    }
    
    public void ToggleMute(bool mute)
    {
        _audioSource.mute = mute;
    }

    public void AdjustSoundVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public void ChangeMusic(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
