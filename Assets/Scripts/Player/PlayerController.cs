using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    bool jumpInput=false;
    [SerializeField]float jumpHeight = 3f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D boxCol2D;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float speed= 5f;

    private float gravityScale = 5f;
    private float fallGravityScale = 15f;


    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKey(KeyCode.Space);
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else
        {
            rb.gravityScale = fallGravityScale;
        }

    }

    private void FixedUpdate()
    { 
        if (isGrounded())
          rb.velocity = new Vector2(horizontalInput * speed*Time.fixedDeltaTime, rb.velocity.y);
        if (jumpInput && isGrounded())
        {
            rb.gravityScale = gravityScale;
            float jumpForce = Mathf.Sqrt(-2 * jumpHeight * (Physics2D.gravity.y * rb.gravityScale)) * rb.mass;
            Debug.Log("Jump Force: "+jumpForce);
            //TODO: adding force mode in addForce Method 
            rb.AddForce(new Vector2(rb.velocity.x,jumpForce*10));
            
        }
       
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCol2D.bounds.center,boxCol2D.bounds.size,0f,Vector2.down,0.4f,whatIsGround);

        return hit.collider != null;
    }
}
