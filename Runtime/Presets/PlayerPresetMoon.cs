using UnityEngine;

/// <summary>
/// Configuration preset pour le joueur avec des parametres type "Lune"
/// (gravite faible, sauts hauts)
/// </summary>
public class PlayerPresetMoon
{
    [Header("Preset: Gravite Lune")]
    [Tooltip("Parametres pour un mouvement type 'Lune' avec gravite reduite")]
    public PlayerPresetData data = new PlayerPresetData
    {
        // Movement
        moveSpeed = 6f,
        groundAcceleration = 15f,
        airAcceleration = 12f,
        turnSpeed = 10f,
        
        // Jump - Higher and floatier
        jumpHeight = 3f,
        coyoteTime = 0.15f,
        jumpBufferTime = 0.15f,
        
        // Gravity - Much lower for moon-like feeling
        gravity = -8f,
        terminalVelocity = 20f
    };
    
    public void ApplyToPlayer(Player player)
    {
        data.ApplyToPlayer(player);
        Debug.Log("Preset 'Gravite Lune' applique au joueur!");
    }
}
