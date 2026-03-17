using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform player;

    public float spawnY = 0f;
    public float distanceBetweenPlatforms = 2.2f;

    public float leftX = -2.8f;
    public float rightX = 2.8f;

    public int startingPlatforms = 10;

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

    void SpawnPlatform()
    {
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

        spawnY += distanceBetweenPlatforms;
    }
}