using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Health h = other.GetComponent<Health>();
        if (h.hp >= h.maxHp) return;     
        h.Heal(1);
        Destroy(gameObject);
    }
}