using UnityEngine;

/// <summary>
/// Configuration preset pour le joueur avec des parametres type "Lune"
/// (gravite faible, sauts hauts)
/// </summary>
[CreateAssetMenu(fileName = "PlayerPresetMoon", menuName = "Stage GTech/Presets/Player Preset Moon")]
public class PlayerPresetMoon : ScriptableObject
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
        terminalVelocity = 20f,
        
        // Ground Check
        groundCheckRadius = 0.3f,
        
        // Camera
        cameraDistance = 6f,
        cameraHeight = 2f,
        mouseSensitivityX = 3f,
        mouseSensitivityY = 2.5f,
        minVerticalAngle = -50f,
        maxVerticalAngle = 70f,
        cameraSmoothTime = 10f
    };
    
    public void ApplyToPlayer(Player player)
    {
        data.ApplyToPlayer(player);
        Debug.Log("Preset 'Gravite Lune' applique au joueur!");
    }
}
