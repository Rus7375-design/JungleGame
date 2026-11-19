using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float speed = 7f;
    public float jumpForce = 12f;
    public int maxHp = 3;
    public int contactDamage = 1;       
    public int blinkCount = 10;         
    public float blinkInterval = 0.2f;  
    public float shootCooldown = 0.3f;
    public int heartHeal = 1; 
    public float fallDeathY = -10f;   
}