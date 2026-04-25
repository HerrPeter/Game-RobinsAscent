using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
  private const int PortraitReferenceWidth = 1080;
  private const int PortraitReferenceHeight = 1920;

  public GameObject settingsPanel;

  public Slider musicSlider;
  public Slider sfxSlider;
  public Toggle fullscreenToggle;

  private int windowedWidth = 608;
  private int windowedHeight = 1080;

  void Awake()
  {
    HideSettingsPanel();
  }
  // Initializes the settings menu by hiding the settings panel and loading the saved preferences for music volume, SFX volume, and fullscreen mode. It also applies these settings to the audio system and screen configuration to ensure that the user's preferences are reflected when they open the settings menu.
  void Start()
  {
    HideSettingsPanel();

    float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
    float savedSfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
    int savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 0);

    if (musicSlider != null)
      musicSlider.value = savedMusic;

    if (sfxSlider != null)
      sfxSlider.value = savedSfx;

    if (fullscreenToggle != null)
    {
      fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
      fullscreenToggle.SetIsOnWithoutNotify(savedFullscreen == 1);
      fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    AdventureAudio.SetMusicVolume(savedMusic);
    ApplyFullscreen(savedFullscreen == 1);
  }

  // Opens the settings panel, allowing the player to adjust audio and display settings. This method is typically called when the player clicks on a "Settings" button in the main menu, providing access to the various options for customizing their gaming experience.
  public void OpenSettings()
  {
    if (settingsPanel != null)
      settingsPanel.SetActive(true);
  }

  // Closes the settings panel, hiding it from view and allowing the player to return to the main menu or previous screen without making changes to the settings.
  public void CloseSettings()
  {
    HideSettingsPanel();
  }

  public void SetMusicVolume(float value)
  {
    PlayerPrefs.SetFloat("MusicVolume", value);
    PlayerPrefs.Save();

    AdventureAudio.SetMusicVolume(value);

    Debug.Log("Music Volume: " + value.ToString("F2") + " Applied: " + AudioVolumeUtility.SliderToSourceVolume(value).ToString("F2"));
  }

  public void SetSFXVolume(float value)
  {
    PlayerPrefs.SetFloat("SFXVolume", value);
    PlayerPrefs.Save();

    Debug.Log("SFX Volume: " + value.ToString("F2") + " Applied: " + AudioVolumeUtility.SliderToSourceVolume(value).ToString("F2"));
  }

  // Sets the fullscreen mode based on the user's choice, saving the preference in PlayerPrefs and applying the appropriate screen settings to ensure the game displays correctly in both fullscreen and windowed modes.
  public void SetFullscreen(bool isFullscreen)
  {
    PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    PlayerPrefs.Save();

    ApplyFullscreen(isFullscreen);
  }

  // Applies the fullscreen setting based on the user's choice, adjusting the screen resolution and mode accordingly to ensure the game displays correctly in both fullscreen and windowed modes.
  void ApplyFullscreen(bool isFullscreen)
  {
    if (isFullscreen)
    {
      if (Screen.fullScreenMode == FullScreenMode.Windowed)
      {
        windowedWidth = Screen.width;
        windowedHeight = Screen.height;
      }

      Resolution displayResolution = Screen.currentResolution;
      Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
      Screen.fullScreen = true;
      Screen.SetResolution(displayResolution.width, displayResolution.height, true);
      return;
    }

    SetPortraitWindowSize();

    Screen.fullScreen = false;
    Screen.fullScreenMode = FullScreenMode.Windowed;
    Screen.SetResolution(windowedWidth, windowedHeight, false);
  }

  // Determines the appropriate window size for portrait mode based on the current display resolution, ensuring that the game maintains a 9:16 aspect ratio while fitting within the screen dimensions.
  void SetPortraitWindowSize()
  {
    Resolution displayResolution = Screen.currentResolution;
    int maxHeight = Mathf.Max(720, displayResolution.height - 120);

    windowedHeight = Mathf.Min(PortraitReferenceHeight, maxHeight);
    windowedWidth = Mathf.RoundToInt(windowedHeight * (9f / 16f));

    if (windowedWidth <= 0)
    {
      windowedWidth = PortraitReferenceWidth;
    }

    if (windowedHeight <= 0)
    {
      windowedHeight = PortraitReferenceHeight;
    }
  }

  void HideSettingsPanel()
  {
    if (settingsPanel != null)
      settingsPanel.SetActive(false);
  }
}
