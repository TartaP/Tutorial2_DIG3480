using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public float checkRadius;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    public Text score;
    public Text Win;
    public Text gameby;
    public Text Lives;

    public Transform groundcheck;
    public LayerMask allGround;

    private int scoreValue = 0;
    private int livesValue = 3;
    private bool facingRight = true;
    private bool isOnGround;
    private float hozMovement;
    private float vertMovement;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        Lives.text = "Lives: " + livesValue.ToString();
        Win.text ="";
        gameby.text ="";
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (isOnGround)
        {
            if (vertMovement == 0.0f && hozMovement == 0.0f)
            {
                anim.SetInteger("State",0);
            }

        }
    }

    void Update()
    {
        hozMovement = Input.GetAxis("Horizontal");
        vertMovement = Input.GetAxis("Vertical");
        
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (isOnGround)
            {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("State",1);
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                anim.SetInteger("State",0);
            }
          
        }
        
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
                transform.position = new Vector2(80.0f, 0.0f);
                livesValue = 3;
                Lives.text = "Lives: " + livesValue.ToString();
            }
        } 
        if (scoreValue >= 8)
        {
            Win.text = "You Win!";
            gameby.text = "Game created by Alan Zeng";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
            speed = 0.0f;

        }
        if (collision.collider.tag == "Enemy")
        {
            livesValue = livesValue - 1;
            Lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue <= 0)
            {
                Win.text = "You Lose!";
                speed = 0.0f;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State",2);
            }
        }
    }

    
}
