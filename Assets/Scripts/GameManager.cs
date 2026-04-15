using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class LevelDefinition
    {
        public string levelName = "Level";
        public float goalHeight = 50f;
        public int requiredCoins = 3;
        public float platformSpacing = 3.2f;
        public int startingPlatforms = 10;
        public float enemySpawnChance = 0.1f;
    }

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
    public TextMeshProUGUI objectiveText;

    //  Level progression variables
    [Header("Level Settings")]
    public LevelDefinition[] levels;

    //  Runtime variables
    [Header("Runtime")]
    public int currentLevel = 1;
    public int coinsCollected = 0;
    public float maxHeightReached = 0f;
    public bool isGameOver = false;
    public bool isTransitioning = false;

    private PlatformSpawner platformSpawner;
    private GameProgressData progressData;
    private LevelDefinition currentLevelData;

    //  Properties to calculate current level goals
    public float CurrentGoalHeight
    {
        get { return currentLevelData != null ? currentLevelData.goalHeight : 0f; }
    }

    //  Calculate required coins for current level
    public int CurrentRequiredCoins
    {
        get { return currentLevelData != null ? currentLevelData.requiredCoins : 0; }
    }

    void Awake()
    {
        Instance = this;
    }

    //  Initialize game state
    void Start()
    {
        if (levels == null || levels.Length == 0)
        {
            levels = BuildDefaultLevels();
        }

        progressData = GameProgressService.Load(levels.Length);
        currentLevel = progressData.currentLevel;

        if (!GameProgressService.IsLevelUnlocked(progressData, currentLevel))
        {
            currentLevel = progressData.highestUnlockedLevel;
            GameProgressService.SetCurrentLevel(progressData, currentLevel);
        }

        currentLevelData = levels[Mathf.Clamp(currentLevel - 1, 0, levels.Length - 1)];
        platformSpawner = FindFirstObjectByType<PlatformSpawner>();

        if (platformSpawner != null)
        {
            platformSpawner.ApplyLevelSettings(currentLevelData);
            platformSpawner.SpawnInitialPlatforms();
        }

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
            CompleteLevel();
        }
        else
        {
            FailLevel();
        }
    }

    //  Handle level transition
    void CompleteLevel()
    {
        if (isTransitioning) return;

        isTransitioning = true;

        GameProgressService.RecordLevelComplete(
            progressData,
            currentLevel,
            coinsCollected,
            maxHeightReached
        );

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = currentLevel >= levels.Length ? "You Cleared Every Level!" : "Level Complete!";
        }

        Invoke(nameof(LoadFollowingScene), 1.5f);
    }

    //  Handle level failure
    void FailLevel()
    {
        if (isGameOver) return;

        isGameOver = true;
        GameProgressService.RecordLevelAttempt(progressData, currentLevel, coinsCollected, maxHeightReached);

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
        GameProgressService.RecordLevelAttempt(progressData, currentLevel, coinsCollected, maxHeightReached);

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

    void LoadFollowingScene()
    {
        isTransitioning = false;

        if (currentLevel >= levels.Length)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

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
            levelText.text = "Level: " + currentLevel + " / " + levels.Length;
        }

        if (objectiveText != null)
        {
            objectiveText.text = currentLevelData.levelName + "  Reach " +
                                 Mathf.FloorToInt(CurrentGoalHeight) +
                                 " height and collect " +
                                 CurrentRequiredCoins +
                                 " gold.";
        }
    }

    LevelDefinition[] BuildDefaultLevels()
    {
        return new LevelDefinition[]
        {
            new LevelDefinition { levelName = "Forest Edge", goalHeight = 45f, requiredCoins = 2, platformSpacing = 3f, startingPlatforms = 10, enemySpawnChance = 0.10f },
            new LevelDefinition { levelName = "Briar Climb", goalHeight = 70f, requiredCoins = 3, platformSpacing = 3.15f, startingPlatforms = 12, enemySpawnChance = 0.18f },
            new LevelDefinition { levelName = "Bandit Lookout", goalHeight = 95f, requiredCoins = 4, platformSpacing = 3.3f, startingPlatforms = 14, enemySpawnChance = 0.24f },
            new LevelDefinition { levelName = "High Canopy", goalHeight = 120f, requiredCoins = 5, platformSpacing = 3.45f, startingPlatforms = 16, enemySpawnChance = 0.30f },
            new LevelDefinition { levelName = "Castle Approach", goalHeight = 145f, requiredCoins = 6, platformSpacing = 3.6f, startingPlatforms = 18, enemySpawnChance = 0.36f }
        };
    }
}
