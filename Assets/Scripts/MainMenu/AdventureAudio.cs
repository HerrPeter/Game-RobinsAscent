using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AdventureAudio : MonoBehaviour
{
  private const string MusicVolumeKey = "MusicVolume";
  private static AdventureAudio instance;

  public AudioClip musicClip;
  public float volume = 0.2f;

  private AudioSource audioSource;

  void Awake()
  {
    audioSource = GetComponent<AudioSource>();

    if (instance != null && instance != this)
    {
      if (musicClip != null)
      {
        instance.SetClipAndPlay(musicClip);
      }

      Destroy(gameObject);
      return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
    audioSource.loop = true;
    audioSource.playOnAwake = true;
    ApplySavedVolume();
    SetClipAndPlay(musicClip);
  }

  void Start()
  {
    if (musicClip != null)
    {
      SetClipAndPlay(musicClip);
    }
  }

  public static void SetMusicVolume(float value)
  {
    if (instance == null || instance.audioSource == null)
    {
      return;
    }

    instance.audioSource.volume = AudioVolumeUtility.SliderToSourceVolume(value);
  }

  void ApplySavedVolume()
  {
    float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, volume);
    audioSource.volume = AudioVolumeUtility.SliderToSourceVolume(savedVolume);
  }

  void SetClipAndPlay(AudioClip clip)
  {
    if (audioSource == null || clip == null)
    {
      return;
    }

    if (audioSource.clip != clip)
    {
      audioSource.clip = clip;
    }

    if (!audioSource.isPlaying)
    {
      audioSource.Play();
    }
  }
}
