using UnityEngine;
using UnityEngine.InputSystem;

public class WallRunning : MonoBehaviour
{
    public float wallRunForce = 10f;
    public float maxWallRunTime = 2f;
    public float wallCheckDistance = 1f;
    public LayerMask wallLayer;
    public float wallJumpForce = 10f;
    public float wallJumpUpwardsForce = 5f;
    public float verticalWallRunSpeed = 5f;

    private bool isWallRunning = false;
    private float wallRunTimer = 0f;
    private Rigidbody rb;
    private Vector3 lastWallNormal;
    private bool isGrounded;
    private PlayerControls playerInputActions;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputActions = new PlayerControls();
    }

    void OnEnable()
    {
        playerInputActions.GamePlay.jump.performed += OnJumpPerformed;
        playerInputActions.Enable();
    }

    void OnDisable()
    {
        playerInputActions.GamePlay.jump.performed -= OnJumpPerformed;
        playerInputActions.Disable();
    }

    void Update()
    {
        CheckForWall();
        if (isWallRunning)
        {
            PerformWallRun();
        }
        else
        {
            wallRunTimer = 0f;
        }
    }

    private void CheckForWall()
    {
        RaycastHit hit;

        // Check for walls on the right and left
        bool wallDetected = Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance, wallLayer) ||
                            Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance, wallLayer);

        if (wallDetected)
        {
            // If a wall is detected, update the wall normal
            lastWallNormal = hit.normal;
            StartWallRun();
        }
        else
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        if (!isWallRunning)
        {
            isWallRunning = true;
            wallRunTimer = 0f;
            rb.useGravity = false; // Disable gravity while wall running
        }
    }

    private void PerformWallRun()
    {
        isWallRunning = true;
        wallRunTimer += Time.deltaTime;

        // Stop wall running if the timer exceeds the max duration
        if (wallRunTimer > maxWallRunTime)
        {
            StopWallRun();
            return;
        }

        // Calculate the direction along the wall
        Vector3 wallRunDirection = Vector3.Cross(lastWallNormal, Vector3.up);

        // Ensure the player moves forward relative to their input direction
        if (Vector3.Dot(wallRunDirection, rb.velocity) < 0)
        {
            wallRunDirection = -wallRunDirection; // Flip direction if necessary
        }

        // Apply force to move the player along the wall
        rb.velocity = new Vector3(wallRunDirection.x * wallRunForce, rb.velocity.y, wallRunDirection.z * wallRunForce);

        // Get vertical movement input (e.g., W/S or joystick vertical axis)
        float verticalInput = playerInputActions.GamePlay.Movement.ReadValue<Vector2>().y;

        // Adjust the player's velocity for vertical movement on the wall
        rb.velocity = new Vector3(rb.velocity.x, verticalInput * verticalWallRunSpeed, rb.velocity.z);

        // Apply a small force towards the wall to keep the player attached
        rb.AddForce(-lastWallNormal * wallRunForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true; // Re-enable gravity
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isWallRunning)
        {
            WallJump();
        }
    }

    private void WallJump()
    {
        // Calculate the jump direction away from the wall
        Vector3 wallJumpDirection = lastWallNormal + Vector3.up * wallJumpUpwardsForce;

        // Apply the jump force
        rb.velocity = new Vector3(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y, wallJumpDirection.z * wallJumpForce);

        // Stop wall running after the jump
        StopWallRun();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}
