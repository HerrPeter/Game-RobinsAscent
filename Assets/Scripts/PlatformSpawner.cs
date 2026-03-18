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

  // Enemy spawning variables
  [Range(0f, 1f)]
  public float enemySpawnChance = 0.1f; // Chance to spawn an enemy on a platform (0 = never, 1 = always)
  public float enemyYOffset = 0.7f; // Vertical offset to position the enemy on top of the platform

  void Start()
  {
    for (int i = 0; i < startingPlatforms; i++)
    {
      SpawnPlatform();
    }
  }

  void Update()
  {
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

    bool spawnRight = Random.value > 0.5f;
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
}