using UnityEngine;

#pragma warning disable 0649

public class Crossbow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    [SerializeField] private float launchForce;

    [SerializeField] private Transform shotPoint;

    [SerializeField] private float cooldownTime = 2.0f;

    [SerializeField] private PlayerController player;
    private float _cooldown;

    public bool facingRight { get; private set; } = true;

    private bool _arrowReady;
    private AudioSource _audioSource;
    [SerializeField] private float radiusLimit = 0.85f;

    [SerializeField] private Transform leftTurnPosition;
    [SerializeField] private Transform rightTurnPosition;

    private Camera _camera;

    // Start is called before the first frame update
    private void Start()
    {
        _camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePosition - bowPosition;
        var transform1 = transform;
        if (direction.x >= radiusLimit)
        {
            transform1.position = rightTurnPosition.position;
            facingRight = true;

        }
        else if (direction.x < -1  * radiusLimit)
        {
            transform1.position = leftTurnPosition.position;
            facingRight = false;
        }
        transform1.right = direction;


        if (_cooldown <= 0)
        {
            _arrowReady = true;
        }
        else
        {
            _cooldown -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && _arrowReady)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        _audioSource.Play();
        _cooldown = cooldownTime;
        _arrowReady = false;
    }
}