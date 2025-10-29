using UnityEngine;

/// <summary>
/// Configuration preset pour le joueur avec des parametres type "Mario"
/// (mouvement rapide, precis, responsive)
/// </summary>
[CreateAssetMenu(fileName = "PlayerPresetMario", menuName = "Stage GTech/Presets/Player Preset Mario")]
public class PlayerPresetMario : ScriptableObject
{
    [Header("Preset: Mouvement Mario")]
    [Tooltip("Parametres pour un mouvement rapide et precis type 'Mario'")]
    public PlayerPresetData data = new PlayerPresetData
    {
        // Movement - Fast and responsive
        moveSpeed = 9f,
        groundAcceleration = 30f,
        airAcceleration = 15f,
        turnSpeed = 18f,
        
        // Jump - Quick and snappy
        jumpHeight = 2f,
        coyoteTime = 0.08f,
        jumpBufferTime = 0.1f,
        
        // Gravity - Snappy jumps
        gravity = -25f,
        terminalVelocity = 45f,
        
        // Ground Check
        groundCheckRadius = 0.3f,
        
        // Camera
        cameraDistance = 5f,
        cameraHeight = 1.8f,
        mouseSensitivityX = 3.5f,
        mouseSensitivityY = 3f,
        minVerticalAngle = -50f,
        maxVerticalAngle = 70f,
        cameraSmoothTime = 12f
    };
    
    public void ApplyToPlayer(Player player)
    {
        data.ApplyToPlayer(player);
        Debug.Log("Preset 'Mouvement Mario' applique au joueur!");
    }
}
