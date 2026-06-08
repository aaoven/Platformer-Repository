using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    float h;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    private bool isFacingRight = true;
    private bool jumping;

    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    private bool isWallJumping;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(5f, 5f);

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        h = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {

            jumping = true;
            
        }

        flip();

        wallSlide();
        

    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(h * speed, body.velocity.y);

        if (jumping)
        {
            jump();
        }
        

        if (isWallJumping)
        {
            body.velocity = new Vector2(-h * wallJumpingPower.x, wallJumpingPower.y);

        }
        else
        {
            body.velocity = new Vector2(h * speed, body.velocity.y);
        }
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer); 
    }

    private void wallSlide()
    {
        if (isWalled() && !isGrounded() && Input.GetAxis("Horizontal") != 0f)
        {
            isWallSliding = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else
        {
            isWallSliding = false;
        }
    }

    private void jump()
    {
        if (isGrounded())
        {
            body.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
        else if (isWallSliding)
        {
            isWallJumping = true;
            Invoke("stopWallJumping", wallJumpingDuration);
        }

        jumping = false;
    }

    private void stopWallJumping()
    {
        isWallJumping = false;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void flip()
    {
        if (isFacingRight && Input.GetAxis("Horizontal") < 0f || !isFacingRight && Input.GetAxis("Horizontal") > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale; 
        }
    }
}
