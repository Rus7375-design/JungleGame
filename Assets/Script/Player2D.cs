using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player2D : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;
    public AudioClip coinClip;
    public AudioClip jumpClip;
    public Animator gateAnimator;
    public string nextScene;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Shooter shooter;
    AudioSource src;

    Vector2 move;
    bool onGround;
    bool gateOpen;
    int coinsTotal;
    int startCoins;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
        shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
        startCoins = GameState.coins;
        GameState.available += coinsTotal;
    }

    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(move.x));
        anim.SetBool("Grounded", onGround);

        if (move.x < 0) sr.flipX = false;
        else if (move.x > 0) sr.flipX = true;

        if (transform.position.y < -10f)
            GetComponent<Health>().GameOver();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed) Jump();
    }

    public void OnFire()
    {
        Fire();
    }

    public void SetMoveX(float x)
    {
        move.x = x;
    }

    public void Fire()
    {
        if (Time.timeScale == 0f) return;
        shooter.Shoot();
    }

    public void Jump()
    {
        if (Time.timeScale == 0f) return;
        if (!onGround) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        src.PlayOneShot(jumpClip);
        onGround = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            GameState.coins++;
            src.PlayOneShot(coinClip);

            if (GameState.coins - startCoins >= coinsTotal)
            {
                gateOpen = true;
                if (gateAnimator != null) gateAnimator.SetBool("Open", true);
            }
        }

        if (other.CompareTag("Exit") && gateOpen)
            SceneManager.LoadScene(nextScene);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;

        foreach (ContactPoint2D c in other.contacts)
        {
            if (c.normal.y > 0.5f)
            {
                onGround = true;
                return;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) onGround = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            GetComponent<Health>().TakeDamage(1);
    }
}