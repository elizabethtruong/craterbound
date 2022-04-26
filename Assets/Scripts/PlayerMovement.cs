using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer sprite;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpSFX;
    [SerializeField]
    private AudioClip powerupSFX;
    [SerializeField]
    private AudioClip deathSFX;
    [SerializeField]
    private AudioClip mainThemeClip;
    private AudioSource mainTheme;
    bool canDoubleJump;
    public float jumpForce = 14f;

    private static int deaths = 0;
    [SerializeField] private Text deathText;

    private float horizontalInput = 0f;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        mainTheme = GetComponent<AudioSource>();
        mainTheme.clip = mainThemeClip;
        mainTheme.Play();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                anim.SetTrigger("jump");
                canDoubleJump = true;
                audioSource.PlayOneShot(jumpSFX);
            }
            else if (canDoubleJump)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce * .66f);
                anim.SetTrigger("jump");
                canDoubleJump = false;
                audioSource.PlayOneShot(jumpSFX);
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

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Powerup")
        {
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(powerupSFX);
            jumpForce = 20f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }

        if (collision.tag == "Trap") 
        {
            Die();
            deaths++;
            deathText.text = "Deaths: " + deaths;
        }

        if (collision.tag == "Checkpoint") 
        {
            mainTheme.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Enemy") 
        {
            Die();
            deaths++;
            deathText.text = "Deaths: " + deaths;
        }
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 14f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Die()
    {
        if (mainTheme.isPlaying) {
            mainTheme.Stop();
            audioSource.PlayOneShot(deathSFX);
            GetComponent<SpriteRenderer>().color = Color.red;
            body.bodyType = RigidbodyType2D.Static;
            StartCoroutine(LoadSceneWithDelay());
        }
    }
}
