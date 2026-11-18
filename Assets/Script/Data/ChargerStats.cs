using UnityEngine;

[CreateAssetMenu(fileName = "ChargerStats", menuName = "Game/Charger Stats")]
public class ChargerStats : ScriptableObject
{
    public float triggerRange = 6f;
    public float fov = 120f;
    public float windUp = 0.7f;
    public float dashSpeed = 9f;
    public float dashTime = 0.6f;
    public float stun = 1f;
    public float returnSpeed = 3f;
    public int maxHp = 3;
}