using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Game/Boss Stats")]
public class BossStats : ScriptableObject
{
    public int maxHp = 10;
    public float range = 14f;          
    public float cooldown = 1.5f;     
    public int bulletsPerVolley = 3;
    public float spread = 15f;         
    public int contactDamage = 1;
}