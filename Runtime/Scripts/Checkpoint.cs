using UnityEngine;

/// <summary>
/// Simple checkpoint that can be dropped in the level. When the player enters the
/// trigger we inform the GameManager and optionally play a feedback.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("Si active ce checkpoint devient automatiquement le point de depart au chargement de la scene.")]
    public bool isStartingCheckpoint;

    [Tooltip("Effet visuel optionnel joue quand le checkpoint est active.")]
    public ParticleSystem activateEffect;

    [Tooltip("Source audio optionnelle jouee quand le checkpoint est active.")]
    public AudioSource activateAudio;

    private bool isActive;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        Reset();

        if (isStartingCheckpoint)
        {
            var gm = GameManager.Instance;
            if (gm != null)
            {
                gm.OnCheckpointReached(transform);
            }
            isActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var gm = GameManager.Instance;
        if (gm == null)
            return;

        gm.OnCheckpointReached(transform);

        if (!isActive)
        {
            isActive = true;
            if (activateEffect != null)
                activateEffect.Play();
            if (activateAudio != null)
                activateAudio.Play();
        }
    }

    private void OnDrawGizmos()
    {
        bool active = Application.isPlaying ? isActive : isStartingCheckpoint;

        Color color = active ? new Color(0.2f, 0.9f, 0.3f, 1f) : Color.yellow;
        color.a = 0.35f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
