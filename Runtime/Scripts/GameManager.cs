using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public Transform checkpoint;
    public Transform player;

    public int score;
    public int life;

    public void OnCheckpoint(Transform newCheckpoint)
    {
        checkpoint = newCheckpoint;
    }

    public void OnDeath()
    {
        player.position = checkpoint.position;
        
        life--;
    }
}
