using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
  public GameObject pausePanel;
  public GameObject startPanel;

  private bool isPaused = false;
  private bool isAwaitingStart = false;

  void Start()
  {
    if (pausePanel != null)
    {
      pausePanel.SetActive(false);
    }

    ShowStartOverlay();
  }

  void Update()
  {
    if (isAwaitingStart)
    {
      if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
      {
        StartGameFromOverlay();
      }

      return;
    }

    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      if (isPaused)
        ResumeGame();
      else
        PauseGame();
    }
  }

  public void PauseGame()
  {
    isPaused = true;
    if (pausePanel != null)
    {
      pausePanel.SetActive(true);
    }

    Time.timeScale = 0f;
  }

  public void ResumeGame()
  {
    isPaused = false;
    if (pausePanel != null)
    {
      pausePanel.SetActive(false);
    }

    Time.timeScale = 1f;
  }

  public void GoToMainMenu()
  {
    isPaused = false;
    if (pausePanel != null)
    {
      pausePanel.SetActive(false);
    }

    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
  }

  void ShowStartOverlay()
  {
    isAwaitingStart = true;
    isPaused = false;
    Time.timeScale = 0f;

    if (startPanel != null)
    {
      startPanel.SetActive(true);
    }
  }

  void StartGameFromOverlay()
  {
    isAwaitingStart = false;

    if (startPanel != null)
    {
      startPanel.SetActive(false);
    }

    Time.timeScale = 1f;
  }
}
