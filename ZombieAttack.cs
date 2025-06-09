using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    private ZombieMove zombieMove;
    private Animator animator;
    public Transform target;

    void Start()
    {
        zombieMove = GetComponent<ZombieMove>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            // Stop walking and attack
            if (zombieMove != null) zombieMove.canMove = false;
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            // Resume walking
            if (zombieMove != null) zombieMove.canMove = true;
            if (animator != null) animator.SetBool("isWalking", true);
        }
    }
}
