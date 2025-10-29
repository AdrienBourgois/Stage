using UnityEngine;

/// <summary>
/// When the player enters the trigger the requested scene is loaded.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LevelPortal : MonoBehaviour
{
    [Tooltip("Name of the scene to load when the player enters the portal.")]
    public string sceneToLoad;

    [Tooltip("Optional delay before loading the scene. Useful to play a sound or animation.")]
    [Range(0f, 5f)] public float loadDelay = 0f;

    [Tooltip("Play an audio clip when the portal is triggered.")]
    public AudioSource audioSource;

    [Tooltip("Optional VFX played when the portal is triggered.")]
    public ParticleSystem vfx;

    private bool hasTriggered;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        if (audioSource != null)
            audioSource.Play();

        if (vfx != null)
            vfx.Play();

        if (loadDelay > 0f)
        {
            Invoke(nameof(LoadTargetScene), loadDelay);
        }
        else
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        GameManager.Instance?.LoadScene(sceneToLoad);
    }
}
