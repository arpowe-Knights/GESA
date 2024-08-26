using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;
    private MouseControls _mouseControls;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private Rigidbody rb;

    // float xRotation;
    // float yRotation;

    public Transform orientation; // Reference to the orientation transform
    public float moveSpeed;
    public float jumpForce = 5f;

    public float rotationSpeed = 10f; // Speed of rotation based on mouse movement

    void Awake()
    {
        _playerControls = new PlayerControls();
        _mouseControls = new MouseControls();
        rb = GetComponent<Rigidbody>();
        
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnEnable()
    {
        _mouseControls.Enable();
        _playerControls.Enable();
        //_lookAction = _mouseControls.Mouse.looking;
    }
    
    private void OnDisable()
    {
        
        _playerControls.Disable();
    }

    private void Start()
    {
        _moveAction = _playerControls.GamePlay.Movement;
        _lookAction = _mouseControls.Mouse.looking; // Initialize the Look input action
    }

    private void Update()
    {
        // Update the orientation based on mouse input
       // RotateOrientation();

        // Handle jumping
        if (_playerControls.GamePlay.jump.triggered)
        {
            OnJump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }

    private void OnJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // private void RotateOrientation()
    // {
    //     Vector2 lookInput = _lookAction.ReadValue<Vector2>();
    //
    //     // Calculate the rotation based on the mouse movement
    //     float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
    //     float mouseY = lookInput.y * rotationSpeed * Time.deltaTime;
    //     yRotation += mouseX;
    //
    //     xRotation -= mouseY;
    //     
    //     xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    //
    //     // Apply the rotation to the orientation (Yaw - Y axis, Pitch - X axis)
    //     transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    // }
}
