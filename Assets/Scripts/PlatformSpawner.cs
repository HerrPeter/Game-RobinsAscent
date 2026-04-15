using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
  // Platform spawning variables
  public GameObject platformPrefab;
  public GameObject enemyPrefab;
  public Transform player;

  public float spawnY = 0f;
  public float distanceBetweenPlatforms = 3.2f; // Higher value for more spacing, lower for less spacing

  public float leftX = -2.8f;
  public float rightX = 2.8f;

  public int startingPlatforms = 10;

  [Header("Side Variety")]
  public int maxSameSideStreak = 2;

  // Enemy spawning variables
  [Range(0f, 1f)]
  public float enemySpawnChance = 0.1f; // Chance to spawn an enemy on a platform (0 = never, 1 = always)
  public float enemyYOffset = 0.7f; // Vertical offset to position the enemy on top of the platform

  private bool hasSpawnedInitialSet = false;
  private bool hasLastSide;
  private bool lastSpawnWasRight;
  private int sameSideStreak = 0;

  void Start()
  {
  }

  void Update()
  {
    if (!hasSpawnedInitialSet)
    {
      return;
    }

    if (player == null)
    {
      return;
    }

    if (player.position.y + 15f > spawnY)
    {
      SpawnPlatform();
    }
  }

  // Spawns a platform at the current spawnY position, randomly on the left or right side, and optionally spawns an enemy on it
  void SpawnPlatform()
  {
    if (platformPrefab == null)
    {
      Debug.LogError("Platform prefab is missing on PlatformSpawner.");
      return;
    }

    bool spawnRight = ChooseSpawnSide();
    float spawnX = spawnRight ? rightX : leftX;

    GameObject newPlatform = Instantiate(
        platformPrefab,
        new Vector2(spawnX, spawnY),
        Quaternion.identity
    );

    SpriteRenderer sr = newPlatform.GetComponent<SpriteRenderer>();
    if (sr != null)
    {
      sr.flipX = spawnRight;
    }

    if (enemyPrefab != null && Random.value < enemySpawnChance)
    {
      float enemyXOffset = spawnRight ? -0.6f : 0.6f;

      Instantiate(
          enemyPrefab,
          new Vector2(spawnX + enemyXOffset, spawnY + enemyYOffset),
          Quaternion.identity
      );
    }

    spawnY += distanceBetweenPlatforms;
  }

  bool ChooseSpawnSide()
  {
    bool spawnRight = Random.value > 0.5f;

    if (!hasLastSide)
    {
      hasLastSide = true;
      lastSpawnWasRight = spawnRight;
      sameSideStreak = 1;
      return spawnRight;
    }

    if (sameSideStreak >= maxSameSideStreak && spawnRight == lastSpawnWasRight)
    {
      spawnRight = !lastSpawnWasRight;
    }

    if (spawnRight == lastSpawnWasRight)
    {
      sameSideStreak++;
    }
    else
    {
      lastSpawnWasRight = spawnRight;
      sameSideStreak = 1;
    }

    return spawnRight;
  }

  public void ApplyLevelSettings(GameManager.LevelDefinition level)
  {
    if (level == null)
    {
      return;
    }

    distanceBetweenPlatforms = level.platformSpacing;
    startingPlatforms = level.startingPlatforms;
    enemySpawnChance = level.enemySpawnChance;
  }

  public void SpawnInitialPlatforms()
  {
    if (hasSpawnedInitialSet)
    {
      return;
    }

    for (int i = 0; i < startingPlatforms; i++)
    {
      SpawnPlatform();
    }

    hasSpawnedInitialSet = true;
  }
}
