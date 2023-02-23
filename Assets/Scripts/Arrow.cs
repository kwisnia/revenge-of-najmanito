using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D _rb;

    private bool _hasHit;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_hasHit) return;
        var velocity = _rb.velocity;
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null) return;
        _hasHit = true;
        _rb.velocity = Vector2.zero;
        _rb.isKinematic = true;
        gameObject.tag = "ArrowHit";
        var arrowsStuck = GameObject.FindGameObjectsWithTag("ArrowHit");
        if (arrowsStuck.Length > 2)
        {
            Destroy(arrowsStuck[0]);
        }

        Debug.Log("Collision with: " + other.gameObject);
    }

}
