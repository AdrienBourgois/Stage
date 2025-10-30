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

    [Tooltip("Axe sur lequel l'objet doit se deplacer.")]
    public Axis axis = Axis.X;

    [Tooltip("Distance du mouvement en metres (de bout en bout).")]
    [Range(0.1f, 10f)] public float distance = 2f;

    [Tooltip("Temps pour completer un cycle aller-retour (secondes).")]
    [Range(0.1f, 10f)] public float period = 2f;

    [Tooltip("Decalage temporel optionnel pour desynchroniser plusieurs mouvements.")]
    public float timeOffset;

    private Vector3 startPosition;

    private void Start()
    {
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

    private void OnDrawGizmos()
    {
        DrawGizmos(new Color(0.7f, 0.2f, 1f, 0.15f), subtle: true);
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos(new Color(0.85f, 0.2f, 1f, 0.45f), subtle: false);
    }

    private void DrawGizmos(Color color, bool subtle)
    {
        Vector3 basePosition = Application.isPlaying ? startPosition : transform.position;
        Vector3 direction = axis switch
        {
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.right
        };

        float halfDistance = distance * 0.5f;
        Vector3 start = basePosition - direction * halfDistance;
        Vector3 end = basePosition + direction * halfDistance;

        Gizmos.color = color;
        Gizmos.DrawLine(start, end);

        float radius = subtle ? 0.08f : 0.12f;
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);

        Vector3 arrowTip = Vector3.Lerp(start, end, 0.5f);
        Vector3 perp = Vector3.Cross(direction, Vector3.up);
        if (perp.sqrMagnitude < 0.001f)
        {
            perp = Vector3.Cross(direction, Vector3.right);
        }

        perp.Normalize();
        float arrowSize = subtle ? 0.15f : 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + perp * arrowSize);
        Gizmos.DrawLine(arrowTip, arrowTip - perp * arrowSize);
    }
}
