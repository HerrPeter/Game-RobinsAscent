using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AdventureAudio : MonoBehaviour
{
  private const string MusicVolumeKey = "MusicVolume";
  private static AdventureAudio instance;

  public AudioClip musicClip;
  public float volume = 0.2f;

  private AudioSource audioSource;

  // Assigns the music clip and starts playing it, ensuring that only one instance of AdventureAudio exists across scenes.
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
  // Sets the music volume based on the provided slider value, converting it to the appropriate source volume using the AudioVolumeUtility.
  public static void SetMusicVolume(float value)
  {
    if (instance == null || instance.audioSource == null)
    {
      return;
    }

    instance.audioSource.volume = AudioVolumeUtility.SliderToSourceVolume(value);
  }

  // Load the saved music volume from PlayerPrefs and apply it to the audio source, ensuring that the volume is consistent with the user's previous settings.
  void ApplySavedVolume()
  {
    float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, volume);
    audioSource.volume = AudioVolumeUtility.SliderToSourceVolume(savedVolume);
  }

  // Helper method to set the audio clip and play it if it's not already playing, ensuring that the music starts immediately when a new clip is assigned.
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
