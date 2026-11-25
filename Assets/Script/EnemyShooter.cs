using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    enum State { Idle, Keep, Aim, Cooldown }
    State state;
    float timer;

    [Header("Data")]
    [SerializeField] ShooterStats stats;

    [Header("Scene setup")]
    [SerializeField] LayerMask walls;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LineRenderer laser;

    Transform player;
    SpriteRenderer sr;
    Vector2 aimDir;
    float startX, nextLook;
    bool sees;
    int hp;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startX = transform.position.x;
        hp = stats.maxHp;
        if (laser != null) { laser.positionCount = 2; laser.enabled = false; }
    }

    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer()) SetState(State.Keep);
                break;

            case State.Keep:
                sr.flipX = player.position.x > transform.position.x;
                float dx = player.position.x - transform.position.x;
                if (Mathf.Abs(dx) < stats.keepDistance) StepBack(dx);
                if (!CanSeePlayer()) SetState(State.Idle);
                else if (timer >= stats.thinkTime) SetState(State.Aim);
                break;

            case State.Aim:
                if (timer >= stats.aimTime)
                {
                    float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                    Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                if (timer >= stats.cooldown) SetState(State.Keep);
                break;
        }
    }

    bool CanSeePlayer()
    {
        if (Time.time < nextLook) return sees;
        nextLook = Time.time + 0.15f;
        Vector2 toPlayer = player.position - transform.position;
        sees = toPlayer.magnitude <= stats.viewRange
            && !Physics2D.Raycast(transform.position, toPlayer.normalized,
                                  toPlayer.magnitude, walls);
        return sees;
    }

    void StepBack(float dx)
    {
        Vector3 p = transform.position;
        p.x += -Mathf.Sign(dx) * stats.stepSpeed * Time.deltaTime;
        p.x = Mathf.Clamp(p.x, startX - stats.roam, startX + stats.roam);
        transform.position = p;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        bool aim = next == State.Aim;
        if (aim) GameAudio.Play(GameAudio.Bank.windupAim, vary: false);
        sr.color = aim ? Color.red : Color.white;
        if (aim)
        {
            aimDir = (player.position - transform.position).normalized;
            if (laser != null)
            {
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, transform.position + (Vector3)aimDir * stats.viewRange);
            }
        }
        if (laser != null) laser.enabled = aim;
    }

    public void OnShot()
    {
        hp--;
        if (hp <= 0)
        {
            GameAudio.Play(GameAudio.Bank.enemyDeath);
            Destroy(gameObject);
        }
    }
}