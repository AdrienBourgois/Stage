using UnityEngine;

/// <summary>
/// Simple checkpoint that can be dropped in the level. When the player enters the
/// trigger we inform the GameManager and optionally play a feedback.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("If enabled this checkpoint will automatically become the starting point when the scene loads.")]
    public bool isStartingCheckpoint;

    [Tooltip("Optional visual effect triggered when the checkpoint is activated.")]
    public ParticleSystem activateEffect;

    [Tooltip("Optional audio source used to play a sound when the checkpoint is activated.")]
    public AudioSource activateAudio;

    [Header("Debug")]
    public bool drawGizmos = true;
    public Color inactiveColor = Color.yellow;
    public Color activeColor = new Color(0.2f, 0.9f, 0.3f, 1f);

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
        if (!drawGizmos)
            return;

        bool active = Application.isPlaying ? isActive : isStartingCheckpoint;

        Color color = active ? activeColor : inactiveColor;
        color.a = 0.35f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
