using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;
    [HideInInspector] public bool canMove = true; // Other scripts can stop movement

    void Update()
    {
        if (!canMove || target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
