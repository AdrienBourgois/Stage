using UnityEngine;

/// <summary>
/// Configuration preset pour le joueur avec des parametres par defaut equilibres
/// </summary>
[CreateAssetMenu(fileName = "PlayerPresetDefault", menuName = "Stage GTech/Presets/Player Preset Default")]
public class PlayerPresetDefault : ScriptableObject
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
        terminalVelocity = 50f,
        
        // Ground Check
        groundCheckRadius = 0.3f,
        
        // Camera
        cameraDistance = 5f,
        cameraHeight = 1.6f,
        mouseSensitivityX = 3f,
        mouseSensitivityY = 2.5f,
        minVerticalAngle = -50f,
        maxVerticalAngle = 70f,
        cameraSmoothTime = 10f
    };
    
    public void ApplyToPlayer(Player player)
    {
        data.ApplyToPlayer(player);
        Debug.Log("Preset 'Configuration Par Defaut' applique au joueur!");
    }
}
