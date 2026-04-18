using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AdventureAudio : MonoBehaviour
{
  private const string MusicVolumeKey = "MusicVolume";
  private static AdventureAudio instance;

  public AudioClip musicClip;
  public float volume = 0.2f;

  private AudioSource audioSource;
  private float sampleRate = 44100f;

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
    SetClipAndPlay(musicClip != null ? musicClip : CreateTone());
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

    instance.audioSource.volume = Mathf.Clamp01(value);
  }

  void ApplySavedVolume()
  {
    float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, volume);
    audioSource.volume = Mathf.Clamp01(savedVolume);
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

  AudioClip CreateTone()
  {
    int length = (int)sampleRate * 2;
    float[] data = new float[length];

    float[] melody = { 440f, 523f, 659f, 523f }; // simple “adventure” notes

    for (int i = 0; i < length; i++)
    {
      float t = (float)i / sampleRate;
      float note = melody[(i / (length / melody.Length)) % melody.Length];

      data[i] = Mathf.Sin(2 * Mathf.PI * note * t) * 0.3f;
    }

    AudioClip clip = AudioClip.Create("AdventureLoop", length, 1, (int)sampleRate, false);
    clip.SetData(data, 0);

    return clip;
  }
}
