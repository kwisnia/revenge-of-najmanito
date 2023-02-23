
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")] [SerializeField]
    private float speed = 5.0f;

    [Header("Vertical Movement-")] [SerializeField]
    private float jumpHeight = 5.0f;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float jumpDelay = 0.25f;
    [SerializeField] private float coyoteTime;
    [SerializeField] private bool canJump;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private Collider2D crouchDisableCollider;

    private float _horizontal;
    private float _vertical;
    private bool _jumpHeld;
    private float _lookDirection;
    private SpriteRenderer _sp;
    private float _jumpTimer;
    private GameManager _gm;

    private Rigidbody2D _rb;
    private float _hangTimer;
    private bool _isGrounded;
    private bool _facingRight = true;
    private bool _hasJumped;
    private bool _crouching;
    public bool dead { get; set; }
    private Animator _animator;
    private Crossbow _crossbow;
    private bool _controllable = true;
    [SerializeField] private GameObject deathEffect;

    [Header("Ground check")] [SerializeField]
    private Transform leftFootPosition;

    [SerializeField] private Transform rightFootPosition;
    [SerializeField] private Transform arrowLeftCheckPosition;
    [SerializeField] private Transform arrowRightCheckPosition;
    [SerializeField] private Transform headPosition;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsArrow;

    [Header("Audio")] public AudioClip deathClip;
    
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private bool _isCrouchDisableColliderNotNull;
    private static readonly int Crouching = Animator.StringToHash("Crouching");


    private void Awake()
    {
        _isCrouchDisableColliderNotNull = crouchDisableCollider != null;
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _crossbow = GetComponentInChildren<Crossbow>();
        _crossbow.GetComponentInChildren<SpriteRenderer>().enabled = false;
        _gm = FindObjectOfType<GameManager>();
    }

    public void DisableControl()
    {
        _controllable = false;
        _crossbow.enabled = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void EnableControl()
    {
        _controllable = true;
        if (PlayerStats.crossbowCollected)
        {
            _crossbow.enabled = true;
        }

        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    private void Start()
    {
        PlayerStats.LookForCherries();
        if (!PlayerStats.crossbowCollected)
        {
            _crossbow.enabled = false;
        }
        else
        {
            _crossbow.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }

    }

    public void PickUpItem(string itemTag)
    {
        switch (itemTag)
        {
            case "Bow":
                ActivateBow();
                break;
            case "Cherry":
                PlayerStats.AddCherry();
                _gm.DisplayCherries();
                break;
        }
    }

    private void ActivateBow()
    {
        _crossbow.GetComponentInChildren<SpriteRenderer>().enabled = true;
        PlayerStats.crossbowCollected = true;
        _crossbow.enabled = true;
        _gm.SetCheckpoint(transform.position);
        Cursor.visible = true;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("Najman"));
        }
    }

    private void Update()
    {
        if (!_controllable) return;
        
        //Ground check section
        var position = rightFootPosition.position;
        var position1 = leftFootPosition.position;
        var position2 = arrowLeftCheckPosition.position;
        var position3 = arrowRightCheckPosition.position;
        _isGrounded = Physics2D.Raycast(position, Vector2.down, checkRadius, whatIsGround)
                      || Physics2D.Raycast(position, Vector2.down, checkRadius, whatIsArrow)
                      || Physics2D.Raycast(position1, Vector2.down, checkRadius, whatIsGround)
                      || Physics2D.Raycast(position1, Vector2.down, checkRadius, whatIsArrow)
                      || Physics2D.Raycast(position2, Vector2.down, checkRadius, whatIsGround)
                      || Physics2D.Raycast(position2, Vector2.down, checkRadius, whatIsArrow)
                      || Physics2D.Raycast(position3, Vector2.down, checkRadius, whatIsGround)
                      || Physics2D.Raycast(position3, Vector2.down, checkRadius, whatIsArrow);
        
        //Input section
        _horizontal = Input.GetAxis("Horizontal");
        _jumpHeld = Input.GetButton("Jump");
        _vertical = Input.GetAxis("Vertical");

        //Crouch section
        _crouching = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C) || _vertical < 0) && _isGrounded;
        if (!_crouching)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.Raycast(headPosition.position, Vector2.up, checkRadius, whatIsGround))
            {
                _crouching = true;
            }

        }
        
        //Animator section
        _animator.SetFloat(MoveX, Mathf.Abs(_horizontal * speed));
        _animator.SetFloat(MoveY, _rb.velocity.y);
        _animator.SetBool(Grounded, _isGrounded);
        _animator.SetBool(Crouching, _crouching);
        
        // Jump Buffer section
        if (Input.GetButtonDown("Jump"))
        {
            _jumpTimer = Time.time + jumpDelay;

        }
        
        // Coyote Time Section
        canJump = _hangTimer > 0
                  && _rb.velocity.y <= 0.01;
        if (!_isGrounded)
        {
            _hangTimer -= Time.deltaTime;
        }
        else
        {
            _hasJumped = false;
            _hangTimer = coyoteTime;
        }
    }

    private void FixedUpdate()
    {
        if (!_controllable) return;
        _rb.velocity = new Vector2(_horizontal * speed, _rb.velocity.y);
        if (_crouching)
        {
            // Reduce the speed by the crouchSpeed multiplier
            var velocity = _rb.velocity;
            velocity = new Vector2(crouchSpeed * velocity.x, velocity.y);
            _rb.velocity = velocity;


            // Disable one of the colliders when crouching
            if (_isCrouchDisableColliderNotNull)
                crouchDisableCollider.enabled = false;
        } else
        {
            // Enable the collider when not crouching
            if (_isCrouchDisableColliderNotNull)
                crouchDisableCollider.enabled = true;
        }
        if (!_crossbow.enabled)
        {
            if (_horizontal > 0 && !_facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (_horizontal < 0 && _facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        else
        {
            switch (_crossbow.facingRight)
            {
                case true when !_facingRight:
                case false when _facingRight:
                    Flip();
                    break;
            }
        }

        if (_jumpTimer > Time.time && canJump && !_hasJumped)
        {
            Jump();
        }

        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !_jumpHeld)
        {
            _rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            _rb.gravityScale = 1;
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        _jumpTimer = 0;
        _hasJumped = true;
    }


    public void Die()
    {
        if (dead) return;
        dead = true;
        var transform1 = transform;
        Instantiate(deathEffect, transform1.position, transform1.rotation);
        _gm.Restart();
        PlayerStats.AddDeath();
    }
    

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;
        // Flip the sprite.
        _sp.flipX = !_sp.flipX;
    }
}