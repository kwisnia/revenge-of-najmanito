using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager _gm;
    // Start is called before the first frame update
    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;
        Debug.Log("Checkpoint triggered");
        _gm.SetCheckpoint(transform.position);
        GetComponent<Collider2D>().enabled = false;
    }
}
