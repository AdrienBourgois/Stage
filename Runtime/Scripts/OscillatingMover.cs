using UnityEngine;

/// <summary>
/// Simple helper that moves an object back and forth using a cosine wave. Great
/// for moving traps or platforms.
/// </summary>
public class OscillatingMover : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [Tooltip("Axis along which the object should move.")]
    public Axis axis = Axis.X;

    [Tooltip("Distance of the movement in metres (peak to peak).")]
    [Range(0.1f, 10f)] public float distance = 2f;

    [Tooltip("How long it takes to complete a full back and forth cycle (seconds).")]
    [Range(0.1f, 10f)] public float period = 2f;

    [Tooltip("Optional offset to desynchronise multiple movers.")]
    public float timeOffset;

    [Tooltip("Draw a line in the Scene view to visualise the movement range.")]
    public bool drawGizmos = true;

    [Tooltip("Color of the debug line.")]
    public Color gizmoColor = Color.magenta;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnValidate()
    {
        if (period <= 0f)
        {
            period = 0.1f;
        }

        startPosition = transform.position;
    }

    private void Update()
    {
        if (period <= 0f)
            return;

        float t = (Time.time + timeOffset) * Mathf.PI * 2f / period;
        float halfDistance = distance * 0.5f;
        float offset = Mathf.Cos(t) * halfDistance;

        Vector3 target = startPosition;
        switch (axis)
        {
            case Axis.X:
                target.x += offset;
                break;
            case Axis.Y:
                target.y += offset;
                break;
            case Axis.Z:
                target.z += offset;
                break;
        }

        transform.position = target;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Vector3 direction = Vector3.right;
        switch (axis)
        {
            case Axis.Y:
                direction = Vector3.up;
                break;
            case Axis.Z:
                direction = Vector3.forward;
                break;
        }

        Vector3 from = Application.isPlaying ? startPosition : transform.position;
        float halfDistance = distance * 0.5f;
        Vector3 start = from - direction * halfDistance;
        Vector3 end = from + direction * halfDistance;

        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.1f);
        Gizmos.DrawWireSphere(end, 0.1f);
    }
}
