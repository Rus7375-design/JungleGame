using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Game/Projectile Stats")]
public class ProjectileStats : ScriptableObject
{
    public float speed = 12f;
    public float lifetime = 3f;   
    public int damage = 1;       
}