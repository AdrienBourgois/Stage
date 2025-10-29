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

    private void OnDrawGizmosSelected()
    {
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

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.1f);
        Gizmos.DrawWireSphere(end, 0.1f);
    }
}
