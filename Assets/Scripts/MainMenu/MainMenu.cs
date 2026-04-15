using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
  public int totalLevels = 5;
  public TextMeshProUGUI savedLevelText;

  void Start()
  {
    RefreshSavedLevelText();
  }

  public void PlayGame()
  {
    GameProgressData progress = GameProgressService.Load(totalLevels);
    GameProgressService.SetCurrentLevel(progress, progress.currentLevel);
    SceneManager.LoadScene("Game");
  }

  public void StartNewGame()
  {
    RestartProgress();
    SceneManager.LoadScene("Game");
  }

  public void LoadLevel(int levelNumber)
  {
    GameProgressData progress = GameProgressService.Load(totalLevels);

    if (!GameProgressService.IsLevelUnlocked(progress, levelNumber))
    {
      return;
    }

    GameProgressService.SetCurrentLevel(progress, levelNumber);
    SceneManager.LoadScene("Game");
  }

  public void QuitGame()
  {
    Application.Quit();
    Debug.Log("Quit Game");
  }

  public void RestartProgress()
  {
    GameProgressService.Reset(totalLevels);
    RefreshSavedLevelText();
  }

  void RefreshSavedLevelText()
  {
    if (savedLevelText == null)
    {
      return;
    }

    GameProgressData progress = GameProgressService.Load(totalLevels);
    savedLevelText.text = "Saved Level: " + progress.currentLevel + " / " + totalLevels;
  }
}
