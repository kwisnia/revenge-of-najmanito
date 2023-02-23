using UnityEngine;

#pragma warning disable 0649

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private GameObject deathEffect;

    private float _timer;
    private Animator _animator;
    
    private AudioSource _audioSource;
    

    // Start is called before the first frame update
    private void Start()
    {
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        var projectile = other.gameObject.GetComponent<Arrow>();
        var spikes = other.gameObject.GetComponent<DeathCollisions>();

        if (player != null)
        {
            player.Die();
        } else if (projectile != null || spikes != null && GetComponent<FrogController>() != null)
        {
            var transform2 = transform;
            var transform1 = transform2;
            AudioSource.PlayClipAtPoint(deathClip, transform2.position);
            Instantiate(deathEffect, transform1.position, transform1.rotation);
            Destroy(gameObject);
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }
    }
}