using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Vitesse maximale au sol.")]
    [Range(1f, 40f)] public float moveSpeed = 7f;

    [Tooltip("Vitesse d'acceleration au sol.")]
    [Range(1f, 40f)] public float groundAcceleration = 20f;

    [Tooltip("Vitesse d'acceleration en l'air.")]
    [Range(1f, 40f)] public float airAcceleration = 10f;

    [Tooltip("Vitesse de rotation du personnage.")]
    [Range(1f, 30f)] public float turnSpeed = 12f;

    [Tooltip("Angle de rotation a ajouter au player dans le cas ou le mesh ne serait pas dans le bon sens.")]
    [Range(-180f, 180f)] public float rotationOffset = 0f;

    [Header("Jump")]
    [Tooltip("Hauteur du saut en metres.")]
    [Range(0.5f, 50f)] public float jumpHeight = 1.6f;

    [Tooltip("Temps supplementaire pour sauter apres avoir quitte une plateforme.")]
    [Range(0f, 0.5f)] public float coyoteTime = 0.1f;

    [Tooltip("Fenetre de temps pour sauter juste avant d'atterrir.")]
    [Range(0f, 0.5f)] public float jumpBufferTime = 0.1f;

    [Header("Gravity")]
    [Tooltip("Force de gravite appliquee a chaque frame.")]
    [Range(-40f, -5f)] public float gravity = -20f;

    [Tooltip("Vitesse de chute maximale.")]
    [Range(5f, 60f)] public float terminalVelocity = 50f;

    [Header("Camera")]
    [Tooltip("Reference a la camera qui tourne autour du joueur.")]
    public Transform cameraTransform;

    [Tooltip("Distance entre la camera et le joueur.")]
    [Range(2f, 30f)] public float cameraDistance = 5f;

    [Tooltip("Hauteur de la camera au-dessus du pivot du joueur.")]
    [Range(0.2f, 3f)] public float cameraHeight = 1.6f;

    [Tooltip("Sensibilite de la souris sur l'axe X.")]
    [Range(0.5f, 10f)] public float mouseSensitivityX = 3f;

    [Tooltip("Sensibilite de la souris sur l'axe Y.")]
    [Range(0.5f, 10f)] public float mouseSensitivityY = 2.5f;

    [Tooltip("Angle vertical minimum autorise pour la camera.")]
    [Range(-80f, -5f)] public float minVerticalAngle = -50f;

    [Tooltip("Angle vertical maximum autorise pour la camera.")]
    [Range(5f, 85f)] public float maxVerticalAngle = 70f;

    public const float cameraSmoothTime = 40f;

    public CharacterController Controller => controller;

    private CharacterController controller;
    private Vector3 currentVelocity;
    private float verticalVelocity;
    private float yaw;
    private float pitch;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private Vector3 cameraVelocity;

    private bool IsGrounded => controller.isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.RegisterPlayer(this);
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
            targetRotation *= Quaternion.AngleAxis(rotationOffset, Vector3.up);
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
        controller.enabled = false;
        transform.position = position;
        currentVelocity = Vector3.zero;
        verticalVelocity = 0f;
        controller.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 pivot = transform.position + Vector3.up * cameraHeight;
            Gizmos.DrawLine(pivot, cameraTransform.position);
            Gizmos.DrawWireSphere(cameraTransform.position, 0.2f);
        }
    }
}
