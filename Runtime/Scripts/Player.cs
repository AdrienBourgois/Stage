using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("===== Mouvement Joueur =====")]
    [Tooltip("Vitesse de déplacement du joueur (mètres/seconde).")]
    [Range(1f, 100f)] public float moveSpeed = 5f;

    [Tooltip("Force du saut (hauteur maximale du saut).")]
    [Range(2f, 100f)] public float jumpForce = 5f;

    [Tooltip("Force de gravité appliquée vers le bas.")]
    [Range(1f, 20f)] public float gravity = 9.81f;

    [Header("===== Caméra Orbitale =====")]
    [Tooltip("Transform de la caméra principale (doit être assigné manuellement).")]
    public Transform cameraTransform;

    [Tooltip("Distance entre la caméra et le joueur.")]
    [Range(1f, 100f)] public float cameraDistance = 4f;

    [Tooltip("Hauteur de la caméra au-dessus du joueur.")]
    [Range(0.5f, 30f)] public float cameraHeight = 1.6f;

    [Tooltip("Sensibilité de la souris sur l'axe horizontal (X).")]
    [Range(0.1f, 10f)] public float mouseSensitivityX = 3f;

    [Tooltip("Sensibilité de la souris sur l'axe vertical (Y).")]
    [Range(0.1f, 10f)] public float mouseSensitivityY = 2f;

    [Tooltip("Limite minimale de l’angle vertical (regard vers le bas).")]
    [Range(-80f, 0f)] public float minVerticalAngle = 0f;

    [Tooltip("Limite maximale de l’angle vertical (regard vers le haut).")]
    [Range(0f, 85f)] public float maxVerticalAngle = 85f;

    [Tooltip("Vitesse de lissage de la caméra (suivi fluide).")]
    [Range(1f, 100f)] public float cameraSmoothness = 100f;

    bool debugDraw = true;
    Color debugArmColor = Color.cyan;
    Color debugConeColor = new Color(1f, 0.6f, 0f, 0.4f);

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalVelocity;
    private float cameraYaw;
    private float cameraPitch;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleCamera();
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = Quaternion.Euler(0, cameraYaw, 0) * new Vector3(inputX, 0, inputZ);
        moveDirection.Normalize();

        if (controller.isGrounded)
        {
            // Si aucune entrée et sur terrain plat, éviter la glissade
            if (inputX == 0 && inputZ == 0)
                verticalVelocity = 0f;
            else
                verticalVelocity = -0.5f;

            if (Input.GetKeyDown(KeyCode.Space))
                verticalVelocity = jumpForce;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        velocity = moveDirection * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), 0.05f);
        }
    }

    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        cameraYaw += mouseX;
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 desiredPos = transform.position - rotation * Vector3.forward * cameraDistance + Vector3.up * cameraHeight;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPos, Time.deltaTime * cameraSmoothness);
        cameraTransform.rotation = rotation;
    }

    private void OnValidate()
    {
        if (!Application.isPlaying && cameraTransform != null)
        {
            Quaternion previewRot = Quaternion.Euler(cameraPitch, cameraYaw, 0);
            Vector3 previewPos = transform.position - previewRot * Vector3.forward * cameraDistance + Vector3.up * cameraHeight;
            cameraTransform.position = previewPos;
            cameraTransform.rotation = previewRot;
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugDraw || cameraTransform == null)
            return;

        Vector3 playerHead = transform.position + Vector3.up * cameraHeight;
        Gizmos.color = debugArmColor;
        Gizmos.DrawLine(playerHead, cameraTransform.position);
        Gizmos.DrawWireSphere(cameraTransform.position, 0.2f);

        Gizmos.color = debugConeColor;
        Quaternion minRot = Quaternion.Euler(minVerticalAngle, cameraYaw, 0);
        Quaternion maxRot = Quaternion.Euler(maxVerticalAngle, cameraYaw, 0);
        Vector3 minDir = minRot * Vector3.forward * cameraDistance;
        Vector3 maxDir = maxRot * Vector3.forward * cameraDistance;

        Gizmos.DrawRay(playerHead, -minDir);
        Gizmos.DrawRay(playerHead, -maxDir);
        Gizmos.DrawWireSphere(playerHead, 0.1f);
    }
}