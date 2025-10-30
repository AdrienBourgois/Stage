using UnityEngine;

/// <summary>
/// Simple helper that applies a constant rotation around a chosen axis.
/// </summary>
public class Rotator : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [Tooltip("Axe autour duquel appliquer la rotation.")]
    public Axis axis = Axis.Y;

    [Tooltip("Vitesse de rotation en degres par seconde.")]
    [Range(-720f, 720f)] public float speed = 90f;

    [Tooltip("Utilise l'espace local plutot que global pour la rotation.")]
    public bool useLocalSpace = true;

    private void Update()
    {
        Vector3 rotationAxis = Vector3.up;

        switch (axis)
        {
            case Axis.X:
                rotationAxis = Vector3.right;
                break;
            case Axis.Y:
                rotationAxis = Vector3.up;
                break;
            case Axis.Z:
                rotationAxis = Vector3.forward;
                break;
        }

        float angle = speed * Time.deltaTime;
        transform.Rotate(rotationAxis, angle, useLocalSpace ? Space.Self : Space.World);
    }

    private void OnDrawGizmos()
    {
        DrawAxisGizmo(new Color(1f, 0.9f, 0f, 0.15f), 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        DrawAxisGizmo(new Color(1f, 0.8f, 0f, 0.45f), 0.75f);
    }

    private void DrawAxisGizmo(Color color, float length)
    {
        Vector3 localAxis = axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            _ => Vector3.forward
        };

        Vector3 worldAxis = useLocalSpace ? transform.TransformDirection(localAxis) : localAxis;
        Vector3 origin = transform.position;

        Gizmos.color = color;
        Gizmos.DrawLine(origin - worldAxis * length, origin + worldAxis * length);
        Gizmos.DrawSphere(origin + worldAxis * length, 0.05f);
        Gizmos.DrawSphere(origin - worldAxis * length, 0.05f);
    }
}
