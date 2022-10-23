using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource _BGMSource, _SFXSource;
    public SoundDatabase _Database;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _SFXSource.pitch = 1;
        _SFXSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _BGMSource.clip = clip;
        _BGMSource.Play();
    }

    public void PlaySoundRandomPitch(AudioClip clip)
    {
        _SFXSource.pitch = Random.Range(0.5f, 1.8f);
        _SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        _BGMSource.Stop();
    }
}
