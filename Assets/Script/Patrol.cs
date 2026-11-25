using UnityEngine;

public class Patrol : MonoBehaviour
{
    enum State { Patrol, Chase, WindUp, Recover }
    State state;
    float timer;

    [Header("Data")]
    [SerializeField] PatrolStats stats;

    [Header("Scene setup")]
    [SerializeField] Transform pointA, pointB;
    [SerializeField] LayerMask walls;

    Transform player, goal;
    SpriteRenderer sr;
    float minX, maxX, nextLook;
    bool sees;
    int hp;                                   

    int Facing => sr.flipX ? 1 : -1;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        minX = Mathf.Min(pointA.position.x, pointB.position.x);
        maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        goal = pointB;
        hp = stats.maxHp;
    }

    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case State.Patrol:
                MoveX(goal.position.x, stats.patrolSpeed);
                if (Mathf.Abs(transform.position.x - goal.position.x) < 0.05f)
                    goal = goal == pointA ? pointB : pointA;
                if (CanSeePlayer()) SetState(State.Chase);
                break;

            case State.Chase:
                if (CanSeePlayer()) timer = 0f;
                MoveX(Mathf.Clamp(player.position.x, minX, maxX), stats.chaseSpeed);
                if (Close(stats.attackRange)) SetState(State.WindUp);
                else if (timer > stats.loseTime) SetState(State.Patrol);
                break;

            case State.WindUp:
                if (timer >= stats.windUp)
                {
                    if (Close(stats.attackRange + stats.hitExtraRange))
                        player.GetComponent<Health>().TakeDamage(stats.damage);
                    SetState(State.Recover);
                }
                break;

            case State.Recover:
                if (timer >= stats.recover) SetState(State.Chase);
                break;
        }
    }

    bool CanSeePlayer()
    {
        if (Time.time < nextLook) return sees;
        nextLook = Time.time + 0.15f;
        Vector2 toPlayer = player.position - transform.position;
        sees = false;
        if (toPlayer.magnitude > stats.viewRange) return false;
        if (Vector2.Angle(Vector2.right * Facing, toPlayer) > stats.fov / 2f) return false;
        sees = !Physics2D.Raycast(transform.position, toPlayer.normalized,
                                  toPlayer.magnitude, walls);
        return sees;
    }

    bool Close(float range)
    {
        Vector2 d = player.position - transform.position;
        return Mathf.Abs(d.x) < range && Mathf.Abs(d.y) < stats.verticalTolerance;
    }

    void MoveX(float targetX, float speed)
    {
        Vector3 p = transform.position;
        if (Mathf.Abs(targetX - p.x) > 0.01f) sr.flipX = targetX > p.x;
        p.x = Mathf.MoveTowards(p.x, targetX, speed * Time.deltaTime);
        transform.position = p;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        if (next == State.WindUp) GameAudio.Play(GameAudio.Bank.windupMelee, vary: false);
        sr.color = next == State.WindUp ? Color.red : Color.white;
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

    void OnDrawGizmosSelected()
    {
        if (stats == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.viewRange);
    }
}