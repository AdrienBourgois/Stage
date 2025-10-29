using UnityEngine;

/// <summary>
/// Base trap script that respawns the player when they collide with it. It also
/// supports playing sounds or particles.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [Tooltip("Optional delay before the respawn happens (seconds).")]
    [Range(0f, 3f)] public float respawnDelay;

    [Tooltip("Audio played when the trap is triggered.")]
    public AudioSource audioSource;

    [Tooltip("Optional particle effect played on trigger.")]
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
