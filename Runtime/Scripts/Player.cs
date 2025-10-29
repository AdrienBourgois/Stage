using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Maximum speed while grounded.")]
    [Range(1f, 15f)] public float moveSpeed = 7f;

    [Tooltip("How fast the character accelerates when on the ground.")]
    [Range(1f, 30f)] public float groundAcceleration = 20f;

    [Tooltip("How fast the character can accelerate while in the air.")]
    [Range(1f, 30f)] public float airAcceleration = 10f;

    [Tooltip("Rotation speed when turning to face the movement direction.")]
    [Range(1f, 30f)] public float turnSpeed = 12f;

    [Header("Jump")]
    [Tooltip("Desired jump height in metres.")]
    [Range(0.5f, 5f)] public float jumpHeight = 1.6f;

    [Tooltip("Extra time the player can still jump after walking off a platform.")]
    [Range(0f, 0.5f)] public float coyoteTime = 0.1f;

    [Tooltip("Buffer window so a jump pressed slightly before landing still works.")]
    [Range(0f, 0.5f)] public float jumpBufferTime = 0.1f;

    [Header("Gravity")]
    [Tooltip("Gravity force applied every frame.")]
    [Range(-40f, -5f)] public float gravity = -20f;

    [Tooltip("Maximum fall speed.")]
    [Range(5f, 60f)] public float terminalVelocity = 50f;

    [Header("Ground Check")]
    [Tooltip("Offset from the player's pivot to perform the ground check.")]
    public Vector3 groundCheckOffset = new Vector3(0f, -0.5f, 0f);

    [Tooltip("Radius of the ground check sphere.")]
    [Range(0.05f, 0.75f)] public float groundCheckRadius = 0.3f;

    [Tooltip("Layers that count as ground.")]
    public LayerMask groundMask = ~0;

    [Header("Camera")]
    [Tooltip("Reference to the camera transform that should orbit around the player.")]
    public Transform cameraTransform;

    [Tooltip("Distance between the camera and the player.")]
    [Range(2f, 12f)] public float cameraDistance = 5f;

    [Tooltip("Height the camera tries to keep above the player's pivot.")]
    [Range(0.2f, 3f)] public float cameraHeight = 1.6f;

    [Tooltip("Mouse sensitivity on the X axis.")]
    [Range(0.5f, 10f)] public float mouseSensitivityX = 3f;

    [Tooltip("Mouse sensitivity on the Y axis.")]
    [Range(0.5f, 10f)] public float mouseSensitivityY = 2.5f;

    [Tooltip("Minimum vertical angle allowed for the camera.")]
    [Range(-80f, -5f)] public float minVerticalAngle = -50f;

    [Tooltip("Maximum vertical angle allowed for the camera.")]
    [Range(5f, 85f)] public float maxVerticalAngle = 70f;

    [Tooltip("How fast the camera reaches its target position.")]
    [Range(1f, 30f)] public float cameraSmoothTime = 10f;

    [Tooltip("Should debug gizmos be drawn in the editor?")]
    public bool drawDebug = true;

    [Tooltip("Color of the ground check gizmo.")]
    public Color groundGizmoColor = new Color(0.8f, 0.4f, 0f, 0.35f);

    [Tooltip("Color of the camera arm gizmo.")]
    public Color cameraGizmoColor = Color.cyan;

    public CharacterController Controller => controller;

    private CharacterController controller;
    private Vector3 currentVelocity;
    private float verticalVelocity;
    private float yaw;
    private float pitch;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private Vector3 cameraVelocity;

    private bool IsGrounded => controller.isGrounded || Physics.CheckSphere(transform.position + groundCheckOffset, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.RegisterPlayer(this);
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        if (cameraTransform != null)
        {
            Vector3 euler = cameraTransform.eulerAngles;
            yaw = euler.y;
            pitch = Mathf.Clamp(euler.x > 180f ? euler.x - 360f : euler.x, minVerticalAngle, maxVerticalAngle);
        }
    }

    private void Update()
    {
        HandleInput(out Vector3 desiredDirection, out bool wantsToJump);
        HandleMovement(desiredDirection, wantsToJump);
        HandleCamera();
    }

    private void HandleInput(out Vector3 desiredDirection, out bool wantsToJump)
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(inputX, 0f, inputZ);
        input = Vector3.ClampMagnitude(input, 1f);

        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            desiredDirection = forward * input.z + right * input.x;
        }
        else
        {
            desiredDirection = input;
        }

        wantsToJump = Input.GetButtonDown("Jump");
    }

    private void HandleMovement(Vector3 desiredDirection, bool wantsToJump)
    {
        bool grounded = IsGrounded;

        if (grounded)
        {
            coyoteCounter = coyoteTime;
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f; // keep the character glued to the ground
            }
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (wantsToJump)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            verticalVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        float acceleration = grounded ? groundAcceleration : airAcceleration;

        Vector3 desiredVelocity = desiredDirection * moveSpeed;
        Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        currentHorizontal = Vector3.MoveTowards(currentHorizontal, desiredVelocity, acceleration * Time.deltaTime);
        currentVelocity = new Vector3(currentHorizontal.x, 0f, currentHorizontal.z);

        verticalVelocity += gravity * Time.deltaTime;
        verticalVelocity = Mathf.Clamp(verticalVelocity, -terminalVelocity, terminalVelocity);

        Vector3 finalVelocity = currentVelocity + Vector3.up * verticalVelocity;
        CollisionFlags flags = controller.Move(finalVelocity * Time.deltaTime);

        if ((flags & CollisionFlags.Below) != 0)
        {
            verticalVelocity = Mathf.Min(verticalVelocity, -2f);
        }

        if (currentHorizontal.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentHorizontal.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    private void HandleCamera()
    {
        if (cameraTransform == null)
            return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition = transform.position + Vector3.up * cameraHeight - cameraRotation * Vector3.forward * cameraDistance;
        cameraTransform.rotation = cameraRotation;
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref cameraVelocity, 1f / cameraSmoothTime);
    }

    public void RespawnAt(Vector3 position)
    {
        StartCoroutine(RespawnRoutine(position));
    }

    private IEnumerator RespawnRoutine(Vector3 position)
    {
        controller.enabled = false;
        transform.position = position;
        currentVelocity = Vector3.zero;
        verticalVelocity = 0f;
        yield return null;
        controller.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawDebug)
            return;

        Gizmos.color = groundGizmoColor;
        Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);

        if (cameraTransform != null)
        {
            Gizmos.color = cameraGizmoColor;
            Vector3 pivot = transform.position + Vector3.up * cameraHeight;
            Gizmos.DrawLine(pivot, cameraTransform.position);
            Gizmos.DrawWireSphere(cameraTransform.position, 0.2f);
        }
    }
}
