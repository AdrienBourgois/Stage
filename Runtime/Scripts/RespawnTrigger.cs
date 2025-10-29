using UnityEngine;

/// <summary>
/// Triggers a respawn when the player enters. Useful for pits, lava or any
/// death volume.
/// </summary>
[RequireComponent(typeof(Collider))]
public class RespawnTrigger : MonoBehaviour
{
    [Tooltip("If true, the trigger will only affect the player. Otherwise every CharacterController is respawned.")]
    public bool playerOnly = true;

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
        if (playerOnly && !other.CompareTag("Player"))
            return;

        if (!playerOnly && !other.TryGetComponent(out CharacterController _))
            return;

        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.OnDeath();
        }
    }
}
