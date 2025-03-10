using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public enum MovementType
{
    Rolling,      // For oval shapes like potatoes
    Tilting       // For straight shapes like carrots
}

public enum MovementAxis
{
    XAxis,        // Movement along X axis
    ZAxis         // Movement along Z axis
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private MovementType movementType = MovementType.Rolling;
    [SerializeField] private MovementAxis movementAxis = MovementAxis.ZAxis;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;
    private float currentHorizontalInput;
    private bool jumpRequested;
    private PlayerVisuals playerVisuals;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerVisuals = GetComponent<PlayerVisuals>();
        inputHandler = GetComponent<PlayerInputHandler>();

        // Subscribe to input events
        inputHandler.OnMovementInput += HandleMovementInput;
        inputHandler.OnJumpInput += HandleJumpInput;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (inputHandler != null)
        {
            inputHandler.OnMovementInput -= HandleMovementInput;
            inputHandler.OnJumpInput -= HandleJumpInput;
        }
    }

    private void HandleMovementInput(Vector2 input)
    {
        //currentHorizontalInput = movementAxis == MovementAxis.XAxis ? input.x : input.y;
        currentHorizontalInput = input.x;
    }

    private void HandleJumpInput()
    {
        if (isGrounded)
        {
            jumpRequested = true;
        }
    }

    private void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        rb.isKinematic = MiniGamesManager.instance.GetIsMiniGameActive();
    }

    private void FixedUpdate()
    {
        Move();

        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
        }
    }

    private void Move()
    {
        // Create movement direction based on selected axis
        Vector3 moveDirection;
        if (movementAxis == MovementAxis.XAxis)
        {
            moveDirection = new Vector3(currentHorizontalInput, 0, 0).normalized;
        }
        else
        {
            moveDirection = new Vector3(0, 0, currentHorizontalInput).normalized;
        }

        // Apply movement to the rigidbody
        Vector3 currentVelocity = rb.linearVelocity;
        if (movementAxis == MovementAxis.XAxis)
        {
            currentVelocity.x = moveDirection.x * moveSpeed;
        }
        else
        {
            currentVelocity.z = moveDirection.z * moveSpeed;
        }
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);

        // Notify visuals about movement
        if (playerVisuals != null)
        {
            playerVisuals.HandleMovement(moveDirection, movementType, movementAxis, isGrounded);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Notify visuals about jump
        if (playerVisuals != null)
        {
            playerVisuals.HandleJump();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    // Public setters for runtime configuration
    public void SetMovementType(MovementType newType)
    {
        movementType = newType;
        playerVisuals?.ResetRotation();
    }

    public void SetMovementAxis(MovementAxis newAxis)
    {
        movementAxis = newAxis;
        playerVisuals?.ResetRotation();
    }
}