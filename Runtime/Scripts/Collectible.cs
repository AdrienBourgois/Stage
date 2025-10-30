using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collectible that awards points to the player upon pickup.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [Tooltip("Nombre de points accordes au joueur lors de la collecte.")]
    [Min(0)] public int points = 1;

    [Tooltip("Evenements additionnels declenches quand la collecte a lieu.")]
    public UnityEvent onCollected;

    private bool consumed;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (consumed || !other.CompareTag("Player"))
            return;

        Player player = other.GetComponent<Player>();

        if (player == null)
        {
            player = other.GetComponentInParent<Player>();
        }

        if (player == null)
            return;

        consumed = true;

        if (points > 0)
        {
            player.AddScore(points);
        }

        onCollected?.Invoke();
        Destroy(gameObject);
    }
}
