using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Drag your player here in Inspector
    public Vector3 offset = new Vector3(0, 2, -5);
    public float mouseSensitivity = 3f;

    float yaw = 0f;
    float pitch = 10f; // start with slight downward look

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -40, 80);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
