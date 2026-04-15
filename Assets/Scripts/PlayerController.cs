using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  private const string SfxVolumeKey = "SFXVolume";

  // Movement variables
  public float moveSpeed = 6f;
  public float jumpForce = 12f;
  public float fallThreshold = 6f;
  public float wrapLeft = -3.5f;
  public float wrapRight = 3.5f;

  // Arrow shooting variables
  public GameObject arrowPrefab;
  public Transform firePoint;
  public float shootCooldown = 0.1f;  // Lower value for faster shooting, higher for slower shooting

  // Internal variables
  private float shootTimer = 0f;
  private bool facingLeft = false;
  private Rigidbody2D rb;
  private float moveInput;
  private GameManager gameManager;
  private SpriteRenderer sr;
  private AudioSource sfxSource;
  private AudioClip shootClip;

  // Initialize references
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    gameManager = FindFirstObjectByType<GameManager>();
    sr = GetComponent<SpriteRenderer>();
    sfxSource = GetComponent<AudioSource>();

    if (sfxSource == null)
    {
      sfxSource = gameObject.AddComponent<AudioSource>();
    }

    sfxSource.playOnAwake = false;
    sfxSource.loop = false;
    sfxSource.spatialBlend = 0f;
    shootClip = CreateShootClip();
  }

  //  Handle player input and movement, as well as screen wrapping and shooting
  void Update()
  {
    // Handle horizontal input
    if (Keyboard.current != null)
    {
      moveInput = 0f;

      if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        moveInput = -1f;

      if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        moveInput = 1f;
    }

    // Apply horizontal movement
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

    if (transform.position.y < Camera.main.transform.position.y - fallThreshold)
    {
      gameManager.GameOver();
    }

    // Handle screen wrapping
    HandleScreenWrap();

    // Flip sprite based on direction
    if (moveInput > 0)
    {
      sr.flipX = false;
      facingLeft = false;
    }
    else if (moveInput < 0)
    {
      sr.flipX = true;
      facingLeft = true;
    }

    // Handle shooting
    shootTimer -= Time.deltaTime;

    if (Keyboard.current.spaceKey.wasPressedThisFrame && shootTimer <= 0f)
    {
      ShootArrow();
      shootTimer = shootCooldown;
    }
  }

  // Handle jumping when colliding with platforms
  void OnCollisionEnter2D(Collision2D collision)
  {
    TryBounce(collision);
  }

  void OnCollisionStay2D(Collision2D collision)
  {
    TryBounce(collision);
  }

  void TryBounce(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag("Platform"))
      return;

    if (rb.linearVelocity.y > 0f)
      return;

    foreach (ContactPoint2D contact in collision.contacts)
    {
      if (contact.normal.y > 0.5f)
      {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        return;
      }
    }
  }

  // Shoot an arrow in the direction the player is facing
  void ShootArrow()
  {
    if (arrowPrefab == null || firePoint == null) return;

    GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

    Arrow arrow = arrowObj.GetComponent<Arrow>();
    if (arrow != null)
    {
      arrow.SetDirection(facingLeft);
    }

    PlayShootSound();
  }

  void PlayShootSound()
  {
    if (sfxSource == null || shootClip == null)
    {
      return;
    }

    float sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
    sfxSource.volume = Mathf.Clamp01(sfxVolume);
    sfxSource.PlayOneShot(shootClip, sfxSource.volume);
  }

  AudioClip CreateShootClip()
  {
    const int sampleRate = 44100;
    const float duration = 0.09f;
    int sampleCount = Mathf.CeilToInt(sampleRate * duration);
    float[] samples = new float[sampleCount];

    for (int i = 0; i < sampleCount; i++)
    {
      float t = (float)i / sampleRate;
      float progress = (float)i / sampleCount;
      float pitch = Mathf.Lerp(1400f, 650f, progress);
      float envelope = (1f - progress) * (1f - progress);
      float tone = Mathf.Sin(2f * Mathf.PI * pitch * t);

      samples[i] = tone * envelope * 0.18f;
    }

    AudioClip clip = AudioClip.Create("ArrowShoot", sampleCount, 1, sampleRate, false);
    clip.SetData(samples, 0);
    return clip;
  }

  void HandleScreenWrap()
  {
    if (transform.position.x < wrapLeft)
    {
      transform.position = new Vector3(wrapRight, transform.position.y, transform.position.z);
    }
    else if (transform.position.x > wrapRight)
    {
      transform.position = new Vector3(wrapLeft, transform.position.y, transform.position.z);
    }
  }
}
