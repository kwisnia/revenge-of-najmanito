using UnityEngine;

#pragma warning disable 0649


public class FrogController : MonoBehaviour
{
    private bool _following;

    private bool _readyToJump = true;
    private float _jumpCooldown;
    [SerializeField] private float timeBetweenJumps;
    [SerializeField] private float fallMultiplier;
    private Rigidbody2D _rb;
    private bool _isGrounded;
    private bool _isJumping;
    [SerializeField] private float jumpHeight;
    private int _direction;
    private Animator _animator;
    
    [SerializeField] private Transform leftFootPosition;
    [SerializeField] private Transform rightFootPosition;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int MoveY = Animator.StringToHash("MoveY");


    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _jumpCooldown = timeBetweenJumps;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_following) return;
        
        var position = rightFootPosition.position;
        var position1 = leftFootPosition.position;
        _isGrounded = Physics2D.Raycast(position1, Vector2.down, checkRadius, whatIsGround)
                      || Physics2D.Raycast(position, Vector2.down, checkRadius, whatIsGround);
        _animator.SetBool(Grounded, _isGrounded);
        _animator.SetFloat(MoveY, _rb.velocity.y);
        if (_readyToJump) return;
        if (_isGrounded)
        {
            _jumpCooldown -= Time.deltaTime;
        }
        if (!(_jumpCooldown <= 0)) return;
        _readyToJump = true;
        _jumpCooldown = timeBetweenJumps;
    }

    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            if (_readyToJump)
            {
                Jump();
            }
            if (_rb.velocity.y < 0)
            {
                _isJumping = false;
            }
        }
        if (!_isJumping)
        {
            _rb.velocity = new Vector2(0, 0);
        }
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = fallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            _following = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            _following = false;
        }    
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;
        if (player.transform.position.x - transform.position.x > 0)
        {
            _direction = 1;
        }
        else
        {
            _direction = -1;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_direction * 5.0f, jumpHeight);
        _readyToJump = false;
        _jumpCooldown = timeBetweenJumps;
        _isJumping = true;
    }
}
