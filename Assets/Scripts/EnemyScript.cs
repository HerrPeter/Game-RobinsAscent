using UnityEngine;

public class Enemy : MonoBehaviour
{
  // Enemy variables
  public GameObject coinPrefab;
  public float bobHeight = 0.12f;
  public float bobSpeed = 1.8f;
  public float swayAngle = 4f;
  public float swaySpeed = 2.4f;

  private Vector3 startPosition;
  private float animationOffset;

  void Start()
  {
    startPosition = transform.position;
    animationOffset = Random.Range(0f, Mathf.PI * 2f);
  }

  void Update()
  {
    float bobOffset = Mathf.Sin((Time.time * bobSpeed) + animationOffset) * bobHeight;
    float swayOffset = Mathf.Sin((Time.time * swaySpeed) + animationOffset) * swayAngle;

    transform.position = new Vector3(startPosition.x, startPosition.y + bobOffset, startPosition.z);
    transform.rotation = Quaternion.Euler(0f, 0f, swayOffset);
  }

  // Call this method when the enemy is defeated
  public void Die()
  {
    if (coinPrefab != null)
    {
      Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    Destroy(gameObject);
  }
}
