using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    private bool isOnGround;
    public float speed;
    public float jumpForce;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public Text score;
    public Text winText;
    public Text livesText;
    public Text loseText;

    public AudioClip musicBackground;
    public AudioClip musicWin;
    public AudioClip musicLose;
    public AudioSource musicSource;

    public GameObject Player;

    private bool facingRight = true;
    private int scoreValue = 0;
    private int lives = 3;

    Animator anim;
   

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = $"Score: {scoreValue.ToString()}";
        livesText.text =$"Lives: {lives.ToString()}"; 
        winText.text = "";
        loseText.text = "";
    
        musicSource.clip = musicBackground;
        musicSource.loop = true;
        musicSource.Play();
        anim = GetComponent<Animator>();
       
        
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }

    // Update is called once per frame
      void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        
        if (!isOnGround)
        {
             anim.SetInteger("State",3);
        } else 
        {
            if (hozMovement !=0) {

        if (hozMovement == 1 || hozMovement == -1) 
            {
                anim.SetInteger("State", 2);
            } else 
            {
                anim.SetInteger("State", 1);
            }
        } 
        else 
        {
            anim.SetInteger("State", 0);
        }
             if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
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

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = $"Score: {scoreValue.ToString()}";
            Destroy(collision.collider.gameObject);
               if (scoreValue == 4)
         {
            transform.position = new Vector3(80.0f, 10.1f, 1.0f);
            lives = 3;
            livesText.text =$"Lives: {lives.ToString()}"; 
            }
            if (scoreValue == 8) 
            {
                musicSource.Stop();
                musicSource.clip = musicWin;
                musicSource.Play();
                musicSource.loop = false;
                winText.text = "You Win! \n Game created by Christopher Loso";
            }
        }

        if (collision.collider.tag == "Enemy") 
        {
            lives -= 1;
            livesText.text = $"Lives: {lives.ToString()}";
            Destroy(collision.collider.gameObject);
            if (lives == 0) 
            {
                musicSource.Stop();
                musicSource.clip = musicLose;
                musicSource.Play();
                musicSource.loop = false;
                loseText.text = "You Lose! Game Created by Christopher Loso";
                Player.SetActive(false);  
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
                
        }
                     
     
    }
}


    
    

