using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
  public GameObject pausePanel;

  private bool isPaused = false;

  void Start()
  {
    if (pausePanel != null)
    {
      pausePanel.SetActive(false);
    }

    Time.timeScale = 1f;
  }

  void Update()
  {
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
    pausePanel.SetActive(true);
    Time.timeScale = 0f;
  }

  public void ResumeGame()
  {
    isPaused = false;
    pausePanel.SetActive(false);
    Time.timeScale = 1f;
  }

  public void GoToMainMenu()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
  }
}