using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    public Transform cam;

    float turnSmoothVelocity;

    [HideInInspector] public bool isPunching = false;
    private bool wasGroundedLastFrame = true;  // 착지 감지용

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPunching) return;

        isGrounded = controller.isGrounded;

        // 착지했을 때 점프 애니메이션 끄기
        if (isGrounded && !wasGroundedLastFrame)
        {
            animator.SetBool("isJumping", false);
        }
        wasGroundedLastFrame = isGrounded;

        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isMoving = direction.magnitude >= 0.1f;

        // 점프 중이 아니면 걷기/달리기 애니메이션 실행
        if (animator != null)
        {
            if (!animator.GetBool("isJumping"))
            {
                animator.SetBool("isWalking", isMoving && !isRunning);
                animator.SetBool("isRunning", isMoving && isRunning);
            }
        }

        if (isMoving && !animator.GetBool("isJumping"))
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        // 점프 입력 처리 및 점프 애니메이션 시작
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void StartPunch()
    {
        isPunching = true;
    }

    public void EndPunch()
    {
        isPunching = false;
    }
}
