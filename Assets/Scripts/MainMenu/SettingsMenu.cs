using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
  public GameObject settingsPanel;

  public Slider musicSlider;
  public Slider sfxSlider;
  public Toggle fullscreenToggle;

  void Start()
  {
    if (settingsPanel != null)
      settingsPanel.SetActive(false);

    float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
    float savedSfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
    int savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1);

    if (musicSlider != null)
      musicSlider.value = savedMusic;

    if (sfxSlider != null)
      sfxSlider.value = savedSfx;

    if (fullscreenToggle != null)
      fullscreenToggle.isOn = savedFullscreen == 1;

    ApplyFullscreen(savedFullscreen == 1);
  }

  public void OpenSettings()
  {
    if (settingsPanel != null)
      settingsPanel.SetActive(true);
  }

  public void CloseSettings()
  {
    if (settingsPanel != null)
      settingsPanel.SetActive(false);
  }

  public void SetMusicVolume(float value)
  {
    PlayerPrefs.SetFloat("MusicVolume", value);
    PlayerPrefs.Save();

    Debug.Log("Music Volume: " + value.ToString("F2"));
  }

  public void SetSFXVolume(float value)
  {
    PlayerPrefs.SetFloat("SFXVolume", value);
    PlayerPrefs.Save();

    Debug.Log("SFX Volume: " + value.ToString("F2"));
  }

  public void SetFullscreen(bool isFullscreen)
  {
    PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    PlayerPrefs.Save();

    ApplyFullscreen(isFullscreen);
  }

  void ApplyFullscreen(bool isFullscreen)
  {
    Screen.fullScreen = isFullscreen;
  }
}