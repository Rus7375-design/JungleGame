using UnityEngine;

[CreateAssetMenu(fileName = "PatrolStats", menuName = "Game/Patrol Stats")]
public class PatrolStats : ScriptableObject
{
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 4.5f;
    public float viewRange = 6f;
    public float fov = 120f;
    public float loseTime = 2f;          
    public float attackRange = 1.5f;
    public float hitExtraRange = 0.3f;   
    public float verticalTolerance = 1f; 
    public float windUp = 0.5f;
    public float recover = 0.7f;
    public int damage = 1;
    public int maxHp = 1;
}