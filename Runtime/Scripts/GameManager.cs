using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple singleton used by the bootcamp exercises to keep track of the player,
/// the current checkpoint and to expose a couple of helper methods such as
/// respawning or loading a new scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
            }

            return instance;
        }
    }

    [Header("Respawn")]
    [Tooltip("Point d'apparition utilise quand aucun checkpoint n'a ete atteint.")]
    [SerializeField] private Transform defaultSpawnPoint;

    [Tooltip("Recherche automatiquement le joueur au demarrage s'il n'est pas assigne manuellement.")]
    [SerializeField] private Player player;

    [Tooltip("Garde ce manager actif lors du chargement de nouvelles scenes.")]
    [SerializeField] private bool persistAcrossScenes;

    private Transform currentCheckpoint;

    public Transform CurrentCheckpoint => currentCheckpoint != null ? currentCheckpoint : defaultSpawnPoint;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (persistAcrossScenes)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }

        if (player != null)
        {
            RegisterPlayer(player);
        }
    }

    /// <summary>
    /// Registers the player to the game manager so we can respawn it later.
    /// </summary>
    public void RegisterPlayer(Player newPlayer)
    {
        player = newPlayer;

        if (currentCheckpoint == null)
        {
            currentCheckpoint = defaultSpawnPoint != null ? defaultSpawnPoint : player.transform;
        }
    }

    /// <summary>
    /// Called by checkpoints when the player walks over them.
    /// </summary>
    public void OnCheckpointReached(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    /// <summary>
    /// Respawns the player at the last activated checkpoint.
    /// </summary>
    public void OnDeath()
    {
        if (player == null)
        {
            Debug.LogWarning("GameManager.OnDeath called but no player is registered yet.");
            return;
        }

        if (CurrentCheckpoint == null)
        {
            Debug.LogWarning("GameManager.OnDeath called but there is no checkpoint or spawn point assigned.");
            return;
        }

        player.HandleDeath();
        player.RespawnAt(CurrentCheckpoint.position);
    }

    /// <summary>
    /// Load another scene (used by portals or buttons).
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Trying to load a scene but the name is empty.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void OnDrawGizmos()
    {
        Transform checkpoint = currentCheckpoint != null ? currentCheckpoint : defaultSpawnPoint;

        if (checkpoint == null)
            return;

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.25f);
        Gizmos.DrawSphere(checkpoint.position, 0.35f);
    }
}
