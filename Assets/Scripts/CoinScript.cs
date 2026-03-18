using UnityEngine;

public class Coin : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (GameManager.Instance != null)
      {
        GameManager.Instance.AddCoin(1);
      }

      Destroy(gameObject);
    }
  }
}