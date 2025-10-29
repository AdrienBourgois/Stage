using UnityEngine;

/// <summary>
/// When the player enters the trigger the requested scene is loaded.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LevelPortal : MonoBehaviour
{
    [Tooltip("Nom de la scene a charger quand le joueur entre dans le portail.")]
    public string sceneToLoad;

    [Tooltip("Delai optionnel avant de charger la scene. Utile pour jouer un son ou une animation.")]
    [Range(0f, 5f)] public float loadDelay = 0f;

    [Tooltip("Joue un clip audio quand le portail est declenche.")]
    public AudioSource audioSource;

    [Tooltip("VFX optionnel joue quand le portail est declenche.")]
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
