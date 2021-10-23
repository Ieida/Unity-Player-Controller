using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Look")]
    public Transform m_camera;
    public float xSensitivity = 2.0f;
    public float ySensitivity = -2.0f;
    public float yMax = 90.0f;
    public float yMin = -90.0f;
    [Header("Move")]
    public CharacterController characterController;
    public float walkSpeed = 1.6f;
    public float gravity = -9.81f;
    // Variables
    float lookX;
    float lookY;
    Vector3 velocity;

    void Start()
    {
        LockCursor();
        CalculateLookValues();
    }

    void Update()
    {
        Look();
        Move();
        Gravity();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CalculateLookValues()
    {
        lookX = Vector3.SignedAngle(Vector3.right, m_camera.right, Vector3.up);
        lookY = Vector3.SignedAngle(Vector3.up, m_camera.up, m_camera.right);
    }

    void Look()
    {
        float xInpt = Input.GetAxisRaw("Mouse X");
        float yInpt = Input.GetAxisRaw("Mouse Y");
        lookX += xInpt * xSensitivity;
        lookY += yInpt * ySensitivity;
        if (lookY > yMax) lookY = yMax;
        else if (lookY < yMin) lookY = yMin;
        m_camera.rotation = Quaternion.Euler(lookY, lookX, 0.0f);
    }

    void Move()
    {
        Vector3 fwd = Vector3.Cross(m_camera.right, Vector3.up);
        Quaternion rot = Quaternion.LookRotation(fwd);
        Vector3 inpt = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0.0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;
        Vector3 mvmnt = rot * inpt * walkSpeed * Time.deltaTime;
        mvmnt += velocity * Time.deltaTime;
        CollisionFlags cllsnFlgs = characterController.Move(mvmnt);
        if (cllsnFlgs == CollisionFlags.Below) velocity.y = 0.0f;
    }

    void Gravity()
    {
        if (!characterController.isGrounded) velocity += Vector3.up * gravity * Time.deltaTime;
    }
}
