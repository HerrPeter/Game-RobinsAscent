using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public Transform player;

    [Header("UI")]
    public TextMeshProUGUI heightText;
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
    public int coinsCollected = 0;
    public float maxHeightReached = 0f;
    public bool isGameOver = false;
    public bool isTransitioning = false;

    public float CurrentGoalHeight
    {
        get { return baseGoalHeight + (currentLevel - 1) * goalHeightIncreasePerLevel; }
    }

    public int CurrentRequiredCoins
    {
        get { return baseRequiredCoins + (currentLevel - 1) * requiredCoinsIncreasePerLevel; }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (player != null)
        {
            maxHeightReached = player.position.y;
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    void Update()
    {
        if (player == null || isGameOver || isTransitioning)
            return;

        if (player.position.y > maxHeightReached)
        {
            maxHeightReached = player.position.y;
        }

        UpdateUI();

        if (maxHeightReached >= CurrentGoalHeight)
        {
            CheckLevelCompletion();
        }
    }

    public void AddCoin(int amount)
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

        Invoke(nameof(AdvanceLevel), 1.5f);
    }

    void AdvanceLevel()
    {
        currentLevel++;
        coinsCollected = 0;

        if (player != null)
        {
            maxHeightReached = player.position.y;
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        isTransitioning = false;
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
        if (heightText != null)
        {
            heightText.text = "Height: " + Mathf.FloorToInt(maxHeightReached) + " / " + Mathf.FloorToInt(CurrentGoalHeight);
        }

        if (coinText != null)
        {
            coinText.text = "Gold: " + coinsCollected + " / " + CurrentRequiredCoins;
        }

        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
    }
}