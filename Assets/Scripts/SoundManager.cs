using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    private AudioSource _audioSource;
    public static SoundManager Instance
    {
        get
        {
            if (!_instance)
            {
                Debug.Log("SoundManager not found");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(!_instance && _instance != this)
        {
           Destroy(gameObject);
           return;
        }

        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void ToggleMute(bool mute)
    {
        _audioSource.mute = mute;
    }

    public void ChangeVolume(float volume)
    {
        _audioSource.volume = volume;
    }
}
