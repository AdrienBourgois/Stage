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
        Player existingPlayer = Player.Instance;

        if (existingPlayer == null)
        {
            existingPlayer = FindFirstObjectByType<Player>();
        }

        if (existingPlayer != null)
        {
            RegisterPlayer(existingPlayer);
        }
        else
        {
            Debug.LogWarning("GameManager could not find a Player in the scene. Please add one.");
        }
    }

    /// <summary>
    /// Registers the player to the game manager so we can respawn it later.
    /// </summary>
    public void RegisterPlayer(Player newPlayer)
    {
        if (newPlayer == null)
            return;

        if (currentCheckpoint == null)
        {
            currentCheckpoint = defaultSpawnPoint != null ? defaultSpawnPoint : newPlayer.transform;
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
        Player activePlayer = Player.Instance;

        if (activePlayer == null)
        {
            activePlayer = FindFirstObjectByType<Player>();
        }

        if (activePlayer == null)
        {
            Debug.LogWarning("GameManager.OnDeath called but no player is registered yet.");
            return;
        }

        if (CurrentCheckpoint == null)
        {
            Debug.LogWarning("GameManager.OnDeath called but there is no checkpoint or spawn point assigned.");
            return;
        }

        activePlayer.HandleDeath();
        activePlayer.RespawnAt(CurrentCheckpoint.position);
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

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
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
