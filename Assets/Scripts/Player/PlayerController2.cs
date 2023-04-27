using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//TODO : implement variable length jump.
public class PlayerController2 : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float fallGravityScale = 15f;
    [SerializeField] float timeToAchieveMaxJump = 3f;
    [SerializeField] float coyoteTime = 0.5f;
    [SerializeField] ParticleSystem dust;
    bool jumpInput = false;
    private float coyoteTimer;

    [Header("Movement,Speed and direction")]
    [SerializeField] private float groundSpeed = 400f;
    [SerializeField] private float accelRate = 30f;
    [SerializeField] private float dccelRate = 60f;
    [SerializeField] private float maxSpeed = 13f;
    float horizontalInput;
    public bool isFacingRight => transform.eulerAngles.y == 0f;
    public bool isFacingLeft => transform.eulerAngles.y == 180f || transform.eulerAngles.y == -180f;
    public bool isFalling => rb.velocity.y < 0f;

    [Header("Ground Check")]
    [SerializeField] float groundCheckDistance = 0.4f;
    [SerializeField] LayerMask whatIsGround;
    private bool wasGrounded = false;

    [Header("Shoot Parameters")]
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private bool allowButtonHold = false;
    public bool isShootPressed => allowButtonHold ? Input.GetKey(shootKey) : Input.GetKeyDown(shootKey);

    float lastJumpPressedTime = 0;


    Rigidbody2D rb;
    BoxCollider2D boxCol2D;
    TrailRenderer trail;
    Animator anim;


    //public Properties
    public bool IsIdle => rb.velocity.x == 0f;
    public bool IsMoving => rb.velocity.x > .1f;
    public bool IsJumpPressed => jumpInput;
    public float MoveSpeed => rb.velocity.x;
    public bool IsLanded => !wasGrounded && isGrounded();

    
    //float jumpPressedTime = 0;

    public static PlayerController2 instance;
    private float currentHorizontalSpeed = 0;

    private int animSpeedHashId;

    private void Awake()
    {
        if(instance == null) instance = this;
        rb = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        anim = GetComponent<Animator>();
    }



    private void Start()
    {
        animSpeedHashId = Animator.StringToHash("Speed");
    }

    private void Update()
    { 
        TakeInput();

        if (IsIdle)
        {
            trail.enabled = false;
        }
        else { 
            
            trail.enabled = true; 
        
        
        }

        HandleAnimations();

        //Debug.Log("Was grounded: " + wasGrounded + " and Is Grounded : " + isGrounded());
        if(!wasGrounded && isGrounded())
        {
            //Debug.Log("Just landed");
            createDust();
        }


        wasGrounded = isGrounded();
        
        handleFlip();
        ClampSpeed();
        //CaluclateSpeed();

        handleCoyote();


        if (jumpInput && coyoteTimer > 0.05f && Time.time - lastJumpPressedTime > 0.2f)
        {
            ////Debug.Log($"Coyote Timer: {coyoteTimer} and Is Grounded: {isGrounded()} and Is Not Jumping: {!isJumping}");
            coyoteTimer = 0;
            jump();
        }
            ////Debug.Log(coyoteTimer);
        handleGravityScale();


        //handleVariabeJump();
    }

    private void FixedUpdate()
    {

        currentHorizontalSpeed = horizontalInput * groundSpeed * Time.fixedDeltaTime;
        rb.velocity = new(currentHorizontalSpeed,rb.velocity.y);
    }

    private void TakeInput()
    {
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void jump()
    {
        //Debug.Log("Jump");
        rb.gravityScale = gravityScale;
        rb.Sleep();
        float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rb.gravityScale) * -2) * rb.mass;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastJumpPressedTime = Time.time;
        //coyoteTimer = 0;
        createDust();
    }

    private void handleGravityScale()
    {
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if(rb.velocity.y < 0 && coyoteTimer < 0.03f)
        {
            rb.gravityScale = fallGravityScale;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size, 0f, Vector2.down, 0.010f, whatIsGround);
       // Debug.DrawRay(transform.position, Vector2.down * 2f + new Vector2(0,boxCol2D.bounds.size.y), Color.black);
        if(hit)
        {
            return true;
        }
        return false;
    }
    private void handleFlip()
    {
        if(horizontalInput < 0f && isFacingRight)
        {
            //Debug.Log("Fliping to 180");
            Flip(180f);
        }
        if(horizontalInput > 0f && isFacingLeft)
        {
            //Debug.Log("Fliping to zero");
            Flip(0);
        }
    }
    private void Flip(float degreeToFlip)
    {
        /*transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z) ;*/

        transform.eulerAngles = new Vector3(0,degreeToFlip,0);

        //Debug.Log("Current angle is: "+ transform.eulerAngles);
        if(isGrounded())
           createDust();
    }

    private void createDust()
    {
        dust.Play();
    }

    private void handleCoyote()
    {
        if (isGrounded()) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;
    }

    /*private void CaluclateSpeed()
    {
        if (horizontalInput != 0)
        {
            currentHorizontalSpeed += horizontalInput * accelRate * Time.deltaTime;
            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -maxSpeed, maxSpeed);
        }

        else
        {
            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, dccelRate * Time.deltaTime);
        }
    }*/

    private void ClampSpeed()
    {
        rb.velocity = new Vector2(Mathf.Clamp(currentHorizontalSpeed, -maxSpeed, maxSpeed),rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "LevelComplete")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }



    private void HandleAnimations()
    {
        if(horizontalInput == 0)
        {
            Debug.Log("Inside zero");
            anim.SetFloat(animSpeedHashId, 0);
        }

        anim.SetFloat(animSpeedHashId, Mathf.Abs(rb.velocity.x));
        Debug.Log(Mathf.Abs(rb.velocity.x));
    }
}
