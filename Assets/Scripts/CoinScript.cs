using UnityEngine;

public class Coin : MonoBehaviour
{
  // When the player collides with the coin, it adds one coin to the player's total in the GameManager and then destroys the coin object, allowing for collection of coins throughout the game.
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