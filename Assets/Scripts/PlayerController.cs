using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public float fallThreshold = 6f;
    public float wrapLeft = -3.5f;
    public float wrapRight = 3.5f;

    private Rigidbody2D rb;
    private float moveInput;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            moveInput = 0f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveInput = -1f;

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveInput = 1f;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (transform.position.y < Camera.main.transform.position.y - fallThreshold)
        {
            gameManager.GameOver();
        }

        if (transform.position.x < wrapLeft)
        {
            transform.position = new Vector3(wrapRight, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > wrapRight)
        {
            transform.position = new Vector3(wrapLeft, transform.position.y, transform.position.z);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (rb.linearVelocity.y <= 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }
}