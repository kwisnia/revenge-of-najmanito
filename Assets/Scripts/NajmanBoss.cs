using UnityEngine;

public class NajmanBoss : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _startPoint;

    [SerializeField] private float speed;
    // Start is called before the first frame update
    private void Start()
    {
        _startPoint = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        var arrow = other.gameObject.GetComponent<Arrow>();
        if (player != null)
        {
            player.Die();
        }

        if (arrow != null)
        {
            Destroy(other.gameObject);
        }
        
    }

    public void ResetPosition()
    {
        transform.position = _startPoint;
    }
}
