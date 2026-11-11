using UnityEngine;

public class Patrol : MonoBehaviour
{
    enum State { Patrol, Chase, WindUp, Recover }
    State state;
    float timer;                                  

    [Header("Moving")]
    [SerializeField] Transform pointA, pointB;    
    [SerializeField] float patrolSpeed = 1.5f;   
    [SerializeField] float chaseSpeed = 4.5f;    

    [Header("Seeing")]
    [SerializeField] float viewRange = 6f;      
    [SerializeField] float fov = 120f;          
    [SerializeField] LayerMask walls;           
    [SerializeField] float loseTime = 2f;  

    [Header("Attack")]
    [SerializeField] float attackRange = 1.5f;   
    [SerializeField] float windUp = 0.5f;       
    [SerializeField] float recover = 0.7f;      
    [SerializeField] int damage = 1;
    [SerializeField] int hp = 1;

    Transform player, goal;
    SpriteRenderer sr;
    float minX, maxX, nextLook;
    bool sees;

    int Facing => sr.flipX ? 1 : -1;       

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;   
        minX = Mathf.Min(pointA.position.x, pointB.position.x);
        maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        goal = pointB;
    }

    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case State.Patrol:
                MoveX(goal.position.x, patrolSpeed);
                if (Mathf.Abs(transform.position.x - goal.position.x) < 0.05f)
                    goal = goal == pointA ? pointB : pointA;
                if (CanSeePlayer()) SetState(State.Chase);
                break;

            case State.Chase:
                if (CanSeePlayer()) timer = 0f;            
                MoveX(Mathf.Clamp(player.position.x, minX, maxX), chaseSpeed);
                if (Close(attackRange)) SetState(State.WindUp);
                else if (timer > loseTime) SetState(State.Patrol);
                break;

            case State.WindUp:                                
                if (timer >= windUp)
                {
                    if (Close(attackRange + 0.3f)) player.GetComponent<Health>().TakeDamage(damage);
                    SetState(State.Recover);
                }
                break;

            case State.Recover:
                if (timer >= recover) SetState(State.Chase);
                break;
        }
    }

    bool CanSeePlayer()
    {
        if (Time.time < nextLook) return sees;              
        nextLook = Time.time + 0.15f;
        Vector2 toPlayer = player.position - transform.position;
        sees = false;
        if (toPlayer.magnitude > viewRange) return false;                                
        if (Vector2.Angle(Vector2.right * Facing, toPlayer) > fov / 2f) return false;    
        sees = !Physics2D.Raycast(transform.position, toPlayer.normalized,
                                  toPlayer.magnitude, walls);                             
        return sees;
    }

    bool Close(float range)
    {
        Vector2 d = player.position - transform.position;
        return Mathf.Abs(d.x) < range && Mathf.Abs(d.y) < 1f;
    }

    bool OnScreen()
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > 0.02f && v.x < 0.98f && v.y > 0.02f && v.y < 0.98f;
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
        sr.color = next == State.WindUp ? Color.red : Color.white;       
    }

    public void OnShot()
    {
        hp--;
        if (hp <= 0) Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}