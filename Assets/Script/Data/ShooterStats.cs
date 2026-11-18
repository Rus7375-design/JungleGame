using UnityEngine;

[CreateAssetMenu(fileName = "ShooterStats", menuName = "Game/Shooter Stats")]
public class ShooterStats : ScriptableObject
{
    public float viewRange = 7f;
    public float keepDistance = 4f;
    public float stepSpeed = 2f;
    public float roam = 2f;
    public float thinkTime = 0.5f;
    public float aimTime = 0.6f;
    public float cooldown = 1.5f;
    public int maxHp = 1;
}