using UnityEngine;

[CreateAssetMenu(fileName = "HazardStats", menuName = "Game/Hazard Stats")]
public class HazardStats : ScriptableObject
{
    public float lifetime = 10f;      
    public float killY = -12f;        
    public int damage = 1;
    public float fuse = 2f;           
    public float explosionLifetime = 0.4f;
    public float explosionGrow = 10f;
}