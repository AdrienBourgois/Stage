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
}
