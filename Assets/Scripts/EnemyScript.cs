using UnityEngine;

public class Enemy : MonoBehaviour
{
  // Enemy variables
  public GameObject coinPrefab;

  public void Die()
  {
    if (coinPrefab != null)
    {
      Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    Destroy(gameObject);
  }
}