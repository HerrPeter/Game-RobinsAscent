using UnityEngine;

public class Arrow : MonoBehaviour
{
  public float speed = 20f; // Higher value for faster arrows, lower for slower arrows
  public float lifetime = 3f;

  private Vector2 direction;

  // Set the direction of the arrow based on player facing
  public void SetDirection(bool facingLeft)
  {
    if (facingLeft)
    {
      direction = new Vector2(-1f, 1f).normalized;
      transform.rotation = Quaternion.Euler(0, 0, 135); // up-left
    }
    else
    {
      direction = new Vector2(1f, 1f).normalized;
      transform.rotation = Quaternion.Euler(0, 0, 45); // up-right
    }
  }

  // Handle collision with enemies
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      Enemy enemy = other.GetComponent<Enemy>();
      if (enemy != null)
      {
        enemy.Die();
      }

      Destroy(gameObject);
    }
  }

  //  Move the arrow and destroy it after its lifetime expires
  void Start()
  {
    Destroy(gameObject, lifetime);
  }

  //  Move the arrow in the set direction
  void Update()
  {
    transform.position += (Vector3)(direction * speed * Time.deltaTime);
  }
}