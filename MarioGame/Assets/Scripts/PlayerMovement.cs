using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 6f;
    private float jumpingPower = 7f;
    private bool isFacingRight = true;
    public Animator animator;
    public bool isCrouching = false;
    void Update()
    {
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        if(horizontal != 0f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("IsJumping", true);

        }
        
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        animator.SetBool("IsJumping",false);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
          
    }
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            animator.SetBool("IsCruching", true);
            speed = 0f;
            jumpingPower = 0f;
        }
        else if (context.canceled)
        {
            animator.SetBool("IsCruching", false);
            speed = 6f;
            jumpingPower = 7f;
        }
    }


}