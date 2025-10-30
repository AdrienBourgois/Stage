using UnityEngine;

/// <summary>
/// Configuration preset pour le joueur avec des parametres par defaut equilibres
/// </summary>
public class PlayerPresetDefault
{
    [Header("Preset: Configuration Par Defaut")]
    [Tooltip("Parametres equilibres pour un bon gameplay general")]
    public PlayerPresetData data = new PlayerPresetData
    {
        // Movement - Balanced
        moveSpeed = 7f,
        groundAcceleration = 20f,
        airAcceleration = 10f,
        turnSpeed = 12f,
        
        // Jump - Balanced
        jumpHeight = 1.6f,
        coyoteTime = 0.1f,
        jumpBufferTime = 0.1f,
        
        // Gravity - Balanced
        gravity = -20f,
        terminalVelocity = 50f
    };
    
    public void ApplyToPlayer(Player player)
    {
        data.ApplyToPlayer(player);
        Debug.Log("Preset 'Configuration Par Defaut' applique au joueur!");
    }
}
