using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //  Game variables
    [Header("Player")]
    public Transform player;

    //  UI variables
    [Header("UI")]
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI messageText;

    //  Level progression variables
    [Header("Level Settings")]
    public int currentLevel = 1;
    public float baseGoalHeight = 50f;
    public float goalHeightIncreasePerLevel = 50f;
    public int baseRequiredCoins = 3;
    public int requiredCoinsIncreasePerLevel = 2;

    //  Runtime variables
    [Header("Runtime")]
    public int coinsCollected = 0;
    public float maxHeightReached = 0f;
    public bool isGameOver = false;
    public bool isTransitioning = false;

    //  Properties to calculate current level goals
    public float CurrentGoalHeight
    {
        get { return baseGoalHeight + (currentLevel - 1) * goalHeightIncreasePerLevel; }
    }

    //  Calculate required coins for current level
    public int CurrentRequiredCoins
    {
        get { return baseRequiredCoins + (currentLevel - 1) * requiredCoinsIncreasePerLevel; }
    }

    void Awake()
    {
        Instance = this;
    }

    //  Initialize game state
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

    //  Main game loop
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

    //  Method to add coins when collected
    public void AddCoin(int amount)
    {
        coinsCollected += amount;
        UpdateUI();
    }

    //  Check if player has met level completion criteria
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

    //  Handle level transition
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

    //  Advance to the next level
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

    //  Handle level failure
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

    // Handle game over scenario
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

    // Restart the current level
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update UI elements with current game state
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