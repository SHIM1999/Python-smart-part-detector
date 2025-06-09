using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    public Transform cam; // Drag your camera here

    float turnSmoothVelocity;

    [HideInInspector] public bool isPunching = false; // <-- This flag!

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPunching) return; // <-- Block movement if punching

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        // --- Camera-relative movement ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        // --- ANIMATION LOGIC ---
        // Tell the Animator when we're moving (for walk/idle)
        bool isWalking = direction.magnitude >= 0.1f;
        if (animator != null)
            animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            // Get angle and rotate player
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

     // --- Animation Event Functions ---
    public void StartPunch()
    {
        isPunching = true;
    }

    public void EndPunch()
    {
        isPunching = false;
    }
}


// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float speed = 5f;
//     public float gravity = -9.81f;
//     public float jumpHeight = 1.5f;

//     private CharacterController controller;
//     private Vector3 velocity;
//     private bool isGrounded;
//     private Animator animator;

//     public Transform cam; // Drag your camera here

//     void Start()
//     {
//         controller = GetComponent<CharacterController>();
//         if (cam == null && Camera.main != null) cam = Camera.main.transform;
//         animator = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         isGrounded = controller.isGrounded;
//         if (isGrounded && velocity.y < 0) velocity.y = -2f;

//         // --- Camera-relative movement ---
//         float x = Input.GetAxis("Horizontal");
//         float z = Input.GetAxis("Vertical");
//         Vector3 direction = new Vector3(x, 0f, z).normalized;

//         if (direction.magnitude >= 0.1f)
//         {
//             // Get angle and rotate player
//             float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
//             float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
//             transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

//             Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
//             controller.Move(moveDir.normalized * speed * Time.deltaTime);
//         }

//         // Jump
//         if (Input.GetButtonDown("Jump") && isGrounded)
//         {
//             velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//         }

//         // Gravity
//         velocity.y += gravity * Time.deltaTime;
//         controller.Move(velocity * Time.deltaTime);
//     }

//     float turnSmoothVelocity;
// }
