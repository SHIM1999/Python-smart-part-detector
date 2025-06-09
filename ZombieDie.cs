using UnityEngine;

public class ZombieDie : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    private int hitCount = 0;
    private int maxHits = 5;  // 5번 맞으면 죽음

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 맞았을 때 호출하는 함수
    public void TakeHit()
    {
        if (isDead) return;

        hitCount++;
        Debug.Log($"Zombie hit! Count: {hitCount}");

        if (hitCount >= maxHits)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("ZombieDie.Die() called!");
        if (isDead) return; // Only die once
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 3f);
    }
}
