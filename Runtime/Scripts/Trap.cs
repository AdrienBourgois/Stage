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
}
