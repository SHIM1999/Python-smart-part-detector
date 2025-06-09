using UnityEngine;

public class PunchHitBox : MonoBehaviour
{
    private Collider hitboxCollider;

    void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false; // Start disabled!
    } 

    // Called by animation events
    public void EnableHitbox()
    {
        hitboxCollider.enabled = true;
        // Debug.Log("Punch hitbox enabled");
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
        // Debug.Log("Punch hitbox disabled");
    }

    // Detects hit with zombie
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PunchHitBox hit: " + other.name);
        if (other.CompareTag("Zombie"))
        {
            Debug.Log("Zombie hit!");
            other.GetComponent<ZombieDie>()?.Die();
        }
    }
}
