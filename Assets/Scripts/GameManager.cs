using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public Transform player;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI messageText;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public float baseGoalHeight = 50f;
    public float goalHeightIncreasePerLevel = 50f;
    public int baseRequiredCoins = 3;
    public int requiredCoinsIncreasePerLevel = 2;

    [Header("Runtime")]
    public float maxHeight = 0f;
    public int coinsCollected = 0;
    public bool isGameOver = false;
    public bool isTransitioning = false;

    public float CurrentGoalHeight => baseGoalHeight + (currentLevel - 1) * goalHeightIncreasePerLevel;
    public int CurrentRequiredCoins => baseRequiredCoins + (currentLevel - 1) * requiredCoinsIncreasePerLevel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);

        if (player != null)
            maxHeight = player.position.y;

        coinsCollected = 0;
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver || isTransitioning || player == null)
            return;

        if (player.position.y > maxHeight)
            maxHeight = player.position.y;

        if (player.position.y >= CurrentGoalHeight)
        {
            CheckLevelCompletion();
        }

        UpdateUI();
    }

    public void AddCoin(int amount = 1)
    {
        coinsCollected += amount;
        UpdateUI();
    }

    void CheckLevelCompletion()
    {
        if (coinsCollected >= CurrentRequiredCoins)
        {
            StartNextLevel();
        }
        else
        {
            FailLevel();
        }
    }

    void StartNextLevel()
    {
        if (isTransitioning) return;

        isTransitioning = true;

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "Level Complete!";
        }

        Invoke(nameof(LoadNextLevelState), 1.5f);
    }

    void LoadNextLevelState()
    {
        currentLevel++;
        coinsCollected = 0;
        maxHeight = player.position.y;
        isTransitioning = false;

        if (messageText != null)
            messageText.gameObject.SetActive(false);

        UpdateUI();
    }

    void FailLevel()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "Not enough gold!";
        }

        Invoke(nameof(RestartGame), 1.5f);
    }

    public void GameOver()
    {
        if (isGameOver || isTransitioning) return;

        isGameOver = true;

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "Game Over";
        }

        Invoke(nameof(RestartGame), 1.5f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Height: " + Mathf.FloorToInt(maxHeight);

        if (coinText != null)
            coinText.text = "Gold: " + coinsCollected + " / " + CurrentRequiredCoins;

        if (levelText != null)
            levelText.text = "Level: " + currentLevel + "  Goal: " + Mathf.FloorToInt(CurrentGoalHeight);
    }
}