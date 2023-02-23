using UnityEngine;

public class Item : MonoBehaviour
{
    private bool _collected;

    [SerializeField] private GameObject pickUpEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected) return;
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;
        _collected = true;
        var transform1 = transform;
        Instantiate(pickUpEffect, transform1.position, transform1.rotation);
        player.PickUpItem(gameObject.tag);
        gameObject.SetActive(false);
    }
}
