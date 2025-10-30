using UnityEngine;

/// <summary>
/// Base trap script that respawns the player when they collide with it. It also
/// supports playing sounds or particles.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [Tooltip("Delai optionnel avant le respawn (secondes).")]
    [Range(0f, 3f)] public float respawnDelay;

    [Tooltip("Audio joue quand le piege est declenche.")]
    public AudioSource audioSource;

    [Tooltip("Effet de particules optionnel joue au declenchement.")]
    public ParticleSystem vfx;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (audioSource != null)
            audioSource.Play();

        if (vfx != null)
            vfx.Play();

        if (respawnDelay > 0f)
        {
            Invoke(nameof(RespawnPlayer), respawnDelay);
        }
        else
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        GameManager.Instance?.OnDeath();
    }

    private void OnDrawGizmos()
    {
        DrawColliderGizmos(new Color(1f, 0.35f, 0f, 0.15f), new Color(1f, 0.45f, 0.05f, 0.65f));
    }

    private void OnDrawGizmosSelected()
    {
        DrawColliderGizmos(new Color(1f, 0.45f, 0.1f, 0.25f), new Color(1f, 0.6f, 0.2f, 0.9f));
    }

    private void DrawColliderGizmos(Color fillColor, Color lineColor)
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
            return;

        Matrix4x4 previousMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        switch (col)
        {
            case BoxCollider box:
                Gizmos.color = fillColor;
                Gizmos.DrawCube(box.center, box.size);
                Gizmos.color = lineColor;
                Gizmos.DrawWireCube(box.center, box.size);
                break;
            case SphereCollider sphere:
                Gizmos.color = fillColor;
                Gizmos.DrawSphere(sphere.center, sphere.radius);
                Gizmos.color = lineColor;
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
                break;
            case CapsuleCollider capsule:
                DrawCapsuleGizmos(capsule, fillColor, lineColor);
                break;
            default:
                Bounds bounds = col.bounds;
                Gizmos.matrix = Matrix4x4.identity;
                Gizmos.color = fillColor;
                Gizmos.DrawCube(bounds.center, bounds.size);
                Gizmos.color = lineColor;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
                break;
        }

        Gizmos.matrix = previousMatrix;
    }

    private void DrawCapsuleGizmos(CapsuleCollider capsule, Color fillColor, Color lineColor)
    {
        float radius = capsule.radius;
        float height = Mathf.Max(capsule.height, radius * 2f);
        Vector3 center = capsule.center;
        Vector3 axis;

        switch (capsule.direction)
        {
            case 0:
                axis = Vector3.right;
                break;
            case 1:
                axis = Vector3.up;
                break;
            default:
                axis = Vector3.forward;
                break;
        }

        float cylinderHeight = Mathf.Max(0f, height - 2f * radius);
        Vector3 offset = axis * (cylinderHeight * 0.5f);

        // Draw body
        if (cylinderHeight > 0f)
        {
            Vector3 size = Vector3.one * radius * 2f;
            if (capsule.direction == 0) size.x = cylinderHeight + 2f * radius;
            if (capsule.direction == 1) size.y = cylinderHeight + 2f * radius;
            if (capsule.direction == 2) size.z = cylinderHeight + 2f * radius;
            Gizmos.color = fillColor;
            Gizmos.DrawCube(center, size);
            Gizmos.color = lineColor;
            Gizmos.DrawWireCube(center, size);
        }

        Vector3 topCenter = center + offset;
        Vector3 bottomCenter = center - offset;

        Gizmos.color = fillColor;
        Gizmos.DrawSphere(topCenter, radius);
        Gizmos.DrawSphere(bottomCenter, radius);

        Gizmos.color = lineColor;
        Gizmos.DrawWireSphere(topCenter, radius);
        Gizmos.DrawWireSphere(bottomCenter, radius);
    }
}
