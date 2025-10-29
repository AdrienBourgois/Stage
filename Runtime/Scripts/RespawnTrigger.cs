using UnityEngine;

/// <summary>
/// Triggers a respawn when the player enters. Useful for pits, lava or any
/// death volume.
/// </summary>
[RequireComponent(typeof(Collider))]
public class RespawnTrigger : MonoBehaviour
{
    [Tooltip("Si vrai le trigger n'affecte que le joueur. Sinon tous les CharacterController sont respawn.")]
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
