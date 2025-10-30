using System.Collections;
using UnityEngine;

/// <summary>
/// Moves the attached object from a start point to an end point when triggered
/// through a UnityEvent-compatible method.
/// </summary>
public class EventMover : MonoBehaviour
{
    [Tooltip("Point de depart du mouvement. L'objet courant est utilise si non renseigne.")]
    public Transform startPoint;

    [Tooltip("Point d'arrivee du mouvement.")]
    public Transform endPoint;

    [Tooltip("Duree du mouvement en secondes.")]
    [Range(0f, 60f)] public float duration = 2f;

    [Tooltip("Courbe optionnelle pour moduler la progression du mouvement.")]
    public AnimationCurve easing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private Coroutine moveRoutine;
    private bool hasCompleted;

    /// <summary>
    /// Methode appelee via UnityEvent pour lancer le mouvement.
    /// </summary>
    public void BeginMove()
    {
        if (hasCompleted)
            return;

        if (moveRoutine != null)
            return;

        if (endPoint == null)
        {
            Debug.LogWarning($"{nameof(EventMover)} on {name} cannot move: end point is missing.");
            return;
        }

        Vector3 from = startPoint != null ? startPoint.position : transform.position;
        Vector3 to = endPoint.position;

        if (duration <= 0f)
        {
            transform.position = to;
            hasCompleted = true;
            return;
        }

        moveRoutine = StartCoroutine(MoveRoutine(from, to));
    }

    private IEnumerator MoveRoutine(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = easing != null ? easing.Evaluate(t) : t;
            transform.position = Vector3.LerpUnclamped(from, to, easedT);
            yield return null;
        }

        transform.position = to;
        hasCompleted = true;
        moveRoutine = null;
    }

    private void OnDrawGizmos()
    {
        DrawPathGizmo(new Color(0.3f, 1f, 0.6f, 0.15f), true);
    }

    private void OnDrawGizmosSelected()
    {
        DrawPathGizmo(new Color(0.1f, 0.9f, 0.4f, 0.35f), false);
    }

    private void DrawPathGizmo(Color color, bool subtle)
    {
        Vector3 origin = startPoint != null ? startPoint.position : transform.position;
        Vector3? target = endPoint != null ? endPoint.position : (Vector3?)null;

        if (target == null)
            return;

        Gizmos.color = color;
        Gizmos.DrawSphere(origin, subtle ? 0.08f : 0.12f);
        Gizmos.DrawSphere(target.Value, subtle ? 0.08f : 0.12f);
        Gizmos.DrawLine(origin, target.Value);

        Vector3 direction = (target.Value - origin).normalized;
        float arrowSize = 0.25f;
        Vector3 right = Vector3.Cross(direction, Vector3.up).normalized * arrowSize * 0.5f;
        Vector3 arrowTip = Vector3.Lerp(origin, target.Value, 0.85f);
        Gizmos.DrawLine(arrowTip, arrowTip - direction * arrowSize + right);
        Gizmos.DrawLine(arrowTip, arrowTip - direction * arrowSize - right);
    }
}
