using UnityEngine;

public class Charger : MonoBehaviour
{
    enum State { Idle, WindUp, Dash, Stunned, Return }
    State state;
    float timer;

    [Header("Seeing")]
    [SerializeField] float triggerRange = 6f;     // m
    [SerializeField] float fov = 120f;            // degrees
    [SerializeField] LayerMask walls;             // tick only Walls

    [Header("Attack")]
    [SerializeField] float windUp = 0.7f;         // s, red warning
    [SerializeField] float dashSpeed = 9f;        // m/s
    [SerializeField] float dashTime = 0.6f;       // s
    [SerializeField] float stun = 1f;             // s, player's counter window
    [SerializeField] float returnSpeed = 3f;      // m/s, walking back home
    [SerializeField] int hp = 3;

    [Header("Look")]
    [SerializeField] Sprite idleSprite;
    [SerializeField] Transform arrow;
    [SerializeField] float arrowOffset = 0.12f;
    [SerializeField] float breathSpeed = 6f;
    [SerializeField] float breathSize = 0.04f;

    Transform player;
    SpriteRenderer sr;
    Animator anim;
    Vector3 baseScale;
    Vector3 homePos;
    bool homeFlip;
    float nextLook;
    bool sees;

    int Facing => sr.flipX ? 1 : -1;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseScale = transform.localScale;
        homePos = transform.position;
        homeFlip = sr.flipX;
        SetState(State.Idle);
    }

    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer() && OnScreen()) SetState(State.WindUp);
                break;

            case State.WindUp:
                if (timer >= windUp) SetState(State.Dash);
                break;

            case State.Dash:
                transform.position += Vector3.right * Facing * dashSpeed * Time.deltaTime;
                if (timer >= dashTime || Blocked()) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer >= stun) SetState(State.Return);
                break;

            case State.Return:
            {
                Vector3 p = transform.position;
                if (Mathf.Abs(homePos.x - p.x) > 0.01f) sr.flipX = homePos.x > p.x;
                p.x = Mathf.MoveTowards(p.x, homePos.x, returnSpeed * Time.deltaTime);
                transform.position = p;
                if (Mathf.Abs(p.x - homePos.x) < 0.05f)
                {
                    sr.flipX = homeFlip;
                    SetState(State.Idle);
                }
                break;
            }
        }
    }

    void LateUpdate()
    {
        float k = state == State.Dash ? 1f : 1f + breathSize * Mathf.Sin(Time.time * breathSpeed);
        transform.localScale = new Vector3(baseScale.x, baseScale.y * k, baseScale.z);

        if (arrow == null) return;
        arrow.localPosition = new Vector3(arrowOffset * Facing, 0f, 0f);
        arrow.localScale = new Vector3(Mathf.Abs(arrow.localScale.x) * Facing,
                                       arrow.localScale.y, 1f);
    }

    bool CanSeePlayer()
    {
        if (Time.time < nextLook) return sees;
        nextLook = Time.time + 0.15f;
        Vector2 toPlayer = player.position - transform.position;
        sees = false;
        if (toPlayer.magnitude > triggerRange) return false;
        if (Vector2.Angle(Vector2.right * Facing, toPlayer) > fov / 2f) return false;
        sees = !Physics2D.Raycast(transform.position, toPlayer.normalized,
                                  toPlayer.magnitude, walls);
        Debug.DrawRay(transform.position, toPlayer, sees ? Color.green : Color.red, 0.15f);
        return sees;
    }

    bool Blocked()
    {
        Vector2 dir = Vector2.right * Facing;
        Vector2 front = (Vector2)transform.position + dir * 0.8f;
        bool wall = Physics2D.Raycast(transform.position, dir, 0.8f, walls);
        bool ground = Physics2D.Raycast(front, Vector2.down, 1.5f, walls);
        return wall || !ground;
    }

    bool OnScreen()
    {
        if (Camera.main == null) return true;
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > 0.02f && v.x < 0.98f && v.y > 0.02f && v.y < 0.98f;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        if (next == State.WindUp) sr.color = Color.red;
        else if (next == State.Stunned) sr.color = Color.gray;
        else sr.color = Color.white;

        bool run = next == State.Dash;
        if (anim != null) anim.enabled = run;
        if (!run && idleSprite != null) sr.sprite = idleSprite;
    }

    public void OnShot()
    {
        hp--;
        if (hp <= 0) Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
        int f = sr != null ? Facing : -1;
        Vector3 l = Quaternion.Euler(0f, 0f, -fov / 2f) * (Vector3.right * f);
        Vector3 r = Quaternion.Euler(0f, 0f,  fov / 2f) * (Vector3.right * f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, l * triggerRange);
        Gizmos.DrawRay(transform.position, r * triggerRange);
    }
}