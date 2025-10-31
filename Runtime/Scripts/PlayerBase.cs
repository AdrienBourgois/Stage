using UnityEngine;

/// <summary>
/// Base class shared by all player variants. Handles common stats such as score
/// and lives, ensures the <see cref="CharacterController"/> cache is ready and
/// registers the player inside the <see cref="GameManager"/> singleton.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public abstract class PlayerBase : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Nombre de vies disponibles pour le joueur au demarrage.")]
    [Range(0, 9)] public int startingLives = 3;

    [Tooltip("Score actuel du joueur (lecture seule).")]
    [SerializeField] private int score;

    [Tooltip("Vies actuellement restantes (lecture seule).")]
    [SerializeField] private int lives;

    protected CharacterController controller;

    public CharacterController Controller => controller;
    public int Score => score;
    public int Lives => lives;

    /// <summary>
    /// Allow derived classes to expose their movement capabilities.
    /// </summary>
    public abstract bool SupportsJumping { get; }

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
        lives = Mathf.Max(0, startingLives);
        score = 0;

        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.RegisterPlayer(this);
        }
    }

    /// <summary>
    /// Override to reset custom movement state (velocity, timers...) on respawn.
    /// </summary>
    protected abstract void ResetState();

    public virtual void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        score += amount;
    }

    public virtual void HandleDeath()
    {
        if (lives <= 0)
            return;

        lives--;
    }

    public virtual void RespawnAt(Vector3 position)
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;

        ResetState();
    }
}
