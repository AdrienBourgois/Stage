using UnityEngine;

/// <summary>
/// Structure de donnees pour stocker les parametres d'un preset de joueur
/// </summary>
[System.Serializable]
public struct PlayerPresetData
{
    // Movement
    public float moveSpeed;
    public float groundAcceleration;
    public float airAcceleration;
    public float turnSpeed;
    
    // Jump
    public float jumpHeight;
    public float coyoteTime;
    public float jumpBufferTime;
    
    // Gravity
    public float gravity;
    public float terminalVelocity;
    
    public void ApplyToPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("Impossible d'appliquer le preset: le joueur est null");
            return;
        }
        
        // Movement
        player.moveSpeed = moveSpeed;
        player.groundAcceleration = groundAcceleration;
        player.airAcceleration = airAcceleration;
        player.turnSpeed = turnSpeed;
        
        // Jump
        player.jumpHeight = jumpHeight;
        player.coyoteTime = coyoteTime;
        player.jumpBufferTime = jumpBufferTime;
        
        // Gravity
        player.gravity = gravity;
        player.terminalVelocity = terminalVelocity;
    }
}
