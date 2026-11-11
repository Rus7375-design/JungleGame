using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    enum State { Idle, Keep, Aim, Cooldown }
    State state;
    float timer;

    [Header("Seeing / moving")]
    [SerializeField] float viewRange = 7f;        
    [SerializeField] LayerMask walls;             
    [SerializeField] float keepDistance = 4f;    
    [SerializeField] float stepSpeed = 2f;      
    [SerializeField] float roam = 2f;        
    [SerializeField] float thinkTime = 0.5f;      

    [Header("Attack")]
    [SerializeField] GameObject bulletPrefab;     
    [SerializeField] LineRenderer laser;          
    [SerializeField] float aimTime = 0.6f;        
    [SerializeField] float cooldown = 1.5f;       
    [SerializeField] int hp = 1;

    Transform player;
    SpriteRenderer sr;
    Vector2 aimDir;
    float startX, nextLook;
    bool sees;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startX = transform.position.x;
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
                if (Mathf.Abs(dx) < keepDistance) StepBack(dx);
                if (!CanSeePlayer()) SetState(State.Idle);
                else if (timer >= thinkTime) SetState(State.Aim);
                break;

            case State.Aim:                                               
                if (timer >= aimTime)
                {
                    float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                    Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                if (timer >= cooldown) SetState(State.Keep);
                break;
        }
    }

    bool CanSeePlayer()
    {
        if (Time.time < nextLook) return sees;
        nextLook = Time.time + 0.15f;
        Vector2 toPlayer = player.position - transform.position;
        sees = toPlayer.magnitude <= viewRange                              
            && !Physics2D.Raycast(transform.position, toPlayer.normalized,
                                  toPlayer.magnitude, walls);              
        return sees;
    }

    void StepBack(float dx)
    {
        Vector3 p = transform.position;
        p.x += -Mathf.Sign(dx) * stepSpeed * Time.deltaTime;
        p.x = Mathf.Clamp(p.x, startX - roam, startX + roam);
        transform.position = p;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        bool aim = next == State.Aim;
        sr.color = aim ? Color.red : Color.white;
        if (aim)
        {
            aimDir = (player.position - transform.position).normalized;  
            if (laser != null)
            {
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, transform.position + (Vector3)aimDir * viewRange);
            }
        }
        if (laser != null) laser.enabled = aim;
    }

    public void OnShot()
    {
        hp--;
        if (hp <= 0) Destroy(gameObject);
    }
}