using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer sprite;
    bool canDoubleJump;

    private float horizontalInput = 0f;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, 14f);
                anim.SetTrigger("jump");
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                body.velocity = new Vector2(body.velocity.x, 14f);
                anim.SetTrigger("jump");
                canDoubleJump = false;
            }
        }
        UpdateAnimationState();
        // Set animator parameters
        anim.SetBool("grounded", isGrounded());
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    private void UpdateAnimationState()
    {

        // Flip player when moving left and right
        if (horizontalInput > 0f)
        {
            anim.SetBool("walk", true);
            transform.localScale = new Vector3(.1f, .1f, .1f);
        }

        else if (horizontalInput < 0f)
        {
            anim.SetBool("walk", true);
            transform.localScale = new Vector3(-.1f, .1f, .1f);
        }

        else
        {
            anim.SetBool("walk", false);
        }
    }
}
