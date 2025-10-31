using UnityEngine;

/// <summary>
/// Alternative player controller for top down platformers. Uses a camera child
/// that follows the player automatically and constrains the movement to the XZ
/// plane with no character rotation.
/// </summary>
public class TopDownPlayer : PlayerBase
{
    [Header("Movement")]
    [Tooltip("Vitesse maximale du joueur sur le plan XZ.")]
    [Range(1f, 25f)] public float moveSpeed = 6f;

    [Tooltip("Acceleration appliquee lorsque l'on maintient une direction.")]
    [Range(1f, 60f)] public float acceleration = 25f;

    [Tooltip("Deceleration appliquee quand aucune direction n'est en cours.")]
    [Range(1f, 60f)] public float deceleration = 30f;

    [Header("Jump")]
    [Tooltip("Hauteur du saut en metres.")]
    [Range(0.5f, 25f)] public float jumpHeight = 1.2f;

    [Tooltip("Temps supplementaire pour sauter apres avoir quitte une plateforme.")]
    [Range(0f, 0.5f)] public float coyoteTime = 0.1f;

    [Tooltip("Fenetre de temps pour sauter juste avant d'atterrir.")]
    [Range(0f, 0.5f)] public float jumpBufferTime = 0.1f;

    [Header("Gravity")]
    [Tooltip("Force de gravite appliquee a chaque frame.")]
    [Range(-40f, -5f)] public float gravity = -20f;

    [Tooltip("Vitesse de chute maximale.")]
    [Range(5f, 60f)] public float terminalVelocity = 40f;

    [Header("Camera")]
    [Tooltip("Camera enfant utilisee pour la vue du dessus. Laisser vide pour la detecter automatiquement.")]
    [SerializeField] private Camera childCamera;

    [Tooltip("Maintient la camera oriente vers le joueur a chaque frame.")]
    public bool keepCameraLookingAtPlayer = true;

    private Vector3 currentVelocity;
    private float verticalVelocity;
    private float coyoteCounter;
    private float jumpBufferCounter;

    private bool IsGrounded => controller.isGrounded;

    public override bool SupportsJumping => true;

    protected override void Awake()
    {
        base.Awake();
        EnsureCameraReference();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 desiredDirection = new Vector3(rawInput.x, 0f, rawInput.y);
        desiredDirection = Vector3.ClampMagnitude(desiredDirection, 1f);

        bool wantsToJump = Input.GetButtonDown("Jump");

        HandleMovement(desiredDirection, wantsToJump);
    }

    private void LateUpdate()
    {
        if (!keepCameraLookingAtPlayer)
            return;

        if (!EnsureCameraReference())
            return;

        Vector3 direction = transform.position - childCamera.transform.position;
        if (direction.sqrMagnitude < 0.0001f)
            return;

        childCamera.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }

    private void HandleMovement(Vector3 desiredDirection, bool wantsToJump)
    {
        bool grounded = IsGrounded;

        if (grounded)
        {
            coyoteCounter = coyoteTime;
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
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

        Vector3 desiredVelocity = desiredDirection * moveSpeed;

        if (desiredDirection.sqrMagnitude > 0.0001f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        verticalVelocity += gravity * Time.deltaTime;
        verticalVelocity = Mathf.Clamp(verticalVelocity, -Mathf.Abs(terminalVelocity), Mathf.Abs(terminalVelocity));

        Vector3 finalVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);
        CollisionFlags flags = controller.Move(finalVelocity * Time.deltaTime);

        if ((flags & CollisionFlags.Below) != 0)
        {
            verticalVelocity = -2f;
        }
    }

    protected override void ResetState()
    {
        currentVelocity = Vector3.zero;
        verticalVelocity = 0f;
        coyoteCounter = 0f;
        jumpBufferCounter = 0f;
    }

    private bool EnsureCameraReference()
    {
        if (childCamera != null)
            return true;

        childCamera = GetComponentInChildren<Camera>();
        return childCamera != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!EnsureCameraReference())
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, childCamera.transform.position);
        Gizmos.DrawWireSphere(childCamera.transform.position, 0.25f);
    }
}
