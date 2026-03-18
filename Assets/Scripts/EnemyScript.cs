using UnityEngine;

public class Enemy : MonoBehaviour
{
  // Enemy variables
  public GameObject coinPrefab;

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