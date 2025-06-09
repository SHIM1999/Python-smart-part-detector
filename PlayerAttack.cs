using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public Collider fistCollider;  // 에디터에서 주먹 콜라이더 연결

    void Start()
    {
        animator = GetComponent<Animator>();
        if (fistCollider != null)
            fistCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
        }
    }

    // 애니메이션 이벤트에서 호출
    public void EnableHitbox()
    {
        if (fistCollider != null)
            fistCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (fistCollider != null)
            fistCollider.enabled = false;
    }

    // // 이 스크립트가 붙은 주먹 콜라이더에서 충돌 감지
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Zombie"))
    //     {
    //         ZombieDie zombie = other.GetComponent<ZombieDie>();
    //         if (zombie != null)
    //         {
    //             zombie.TakeHit();
    //         }
    //     }
    // }
}

