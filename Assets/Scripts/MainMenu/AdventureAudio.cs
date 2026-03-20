using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AdventureAudio : MonoBehaviour
{
  private static AdventureAudio instance;

  public float volume = 0.2f;

  private AudioSource audioSource;
  private float sampleRate = 44100f;
  private float frequency = 440f;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    audioSource.loop = true;
    audioSource.playOnAwake = true;
    audioSource.volume = volume;

    audioSource.clip = CreateTone();
    audioSource.Play();
  }

  void Awake()
  {
    if (instance != null)
    {
      Destroy(gameObject);
      return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
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