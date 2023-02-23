using UnityEngine;

public class DeathCollisions : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Die();
        }
    }
}
