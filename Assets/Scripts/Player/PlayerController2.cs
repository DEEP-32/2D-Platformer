using UnityEngine;

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
    float horizontalInput;
    public bool isFacingRight => transform.localScale.x > 0f;
    public bool isFacingLeft => transform.localScale.x < 0f;
    public bool isFalling => rb.velocity.y < 0f;

    [Header("Ground Check")]
    [SerializeField] float groundCheckDistance = 0.4f;
    [SerializeField] LayerMask whatIsGround;

    [Header("Shoot Parameters")]
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private bool allowButtonHold = false;
    public bool isShootPressed => allowButtonHold ? Input.GetKey(shootKey) : Input.GetKeyDown(shootKey);

    float lastJumpPressedTime = 0;


    Rigidbody2D rb;
    BoxCollider2D boxCol2D;
    
    //float jumpPressedTime = 0;

    public static PlayerController2 instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        rb = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
    }
    private void Update()
    { 
        TakeInput();
        handleFlip();

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
        rb.velocity = new(horizontalInput * groundSpeed * Time.fixedDeltaTime,rb.velocity.y);
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
            Flip();
        }
        if(horizontalInput > 0f && isFacingLeft)
        {
            Flip();
        }
    }
    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z) ;
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


}
