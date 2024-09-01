using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 7f;
    private float jumpingPower = 10f;
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

        if (rb.velocity.y == 0f)
        {
            animator.SetBool("IsJumping", false);
        }
        //if (rb.velocity.x == 0f)
        //{
        //    animator.ResetTrigger("IsRunning");
        //} 
        //if (rb.velocity.x == 0f)
        //{
        //    animator.ResetTrigger("IsRunning");
        //}

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
            animator.SetTrigger("IsRunning"); 
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