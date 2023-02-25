using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public GameObject gmScreen;
    
    private float worldTime = 1; //MULTIPLY THIS WITH TIMEDELTATIME ALWAYS
    private float jumpForce = 400f;
    private float speed = 10f;
    private float currentAcceleration = 1.5f;
    private float gravForce = -15f;

    public Rigidbody2D playerRB;
    
    public bool gameOver;
    public bool isOnGround;
    public bool doubleJump;
    public bool buttery;
    public bool canJump;
    public bool increasedGravity;
    
    
    //visible is the game over texture(which is always following the player), and invisible is a nothing texture
    public Sprite visible, invisible;
    // Start is called before the first frame update
    void Start()
    {
        isOnGround = true;
        gameOver = false;
        //instantiates the rigidbody component of the player for use in code
        playerRB = GetComponent<Rigidbody2D>();
        //tells everything what the game over screen is
        gmScreen = GameObject.FindWithTag("GameOverScreenTag");
        gmScreen.GetComponent<SpriteRenderer>().sprite = invisible;
        doubleJump = false;

        canJump = false;
        worldTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if the game isnt over, it checks for player input/moves the player
        if (!gameOver)
        {
            if (buttery)
            {

                if (Input.GetKey("d"))
                {

                    playerRB.AddForce(transform.right * (currentAcceleration * worldTime));

                    //waits for a sec before accelerating, this is to prevent acceleration from scaling with framerate
                    Invoke("IncreaseAcceleration", 0.1f);
                }
                if (Input.GetKey("a"))
                {

                    playerRB.AddForce(transform.right * -(currentAcceleration * worldTime));

                    //waits for a sec before accelerating, this is to prevent acceleration from scaling with framerate
                    Invoke("IncreaseAcceleration", 0.1f);
                }
            }
            
            //Basic movement

            //timedeltatime is multiplied with worldtime first because if its not, getaxishorizontal will be reversed if it happens to be negative
            transform.Translate(Vector3.right * speed * ((Time.deltaTime % 10) * worldTime) * Input.GetAxis("Horizontal"));
            if (Input.GetKeyDown("space"))
            {
                //seperated the if statements for preformance reasons
                if (isOnGround)
                {
                playerRB.AddForce(transform.up * jumpForce * worldTime);
                isOnGround = false;
                increasedGravity = false;
                Invoke("WaitASecond", 0.3f);


                }
                if (doubleJump && canJump)
                {
                playerRB.AddForce(transform.up * (jumpForce * 1.5f) * worldTime);
                canJump = false;
                Invoke("IncreasedGravity", 0.4f);
                

                }
                
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the object is ground they can jump off of
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            canJump = false;
            

        }
        //if the object is an instakill
        if (collision.gameObject.CompareTag("Instakill"))
        {
            //pauses every pausible thing
            worldTime = 0;

            //GameOver Screen play
            gameOver = true;
            gmScreen.GetComponent<SpriteRenderer>().sprite = visible;

        }
        if (collision.gameObject.CompareTag("DoubleJump"))//if they touch a double jump powerup
        {
            doubleJump = true;
            speed = 10;
            buttery = false;
            //destroys the object with the tag double jump
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Buttery"))//if they touch a double jump powerup
        {
            buttery = true;
            doubleJump = false;
            //destroys the object with the tag double jump
            Destroy(collision.gameObject);
            speed = 0;
        }
        if (collision.gameObject.CompareTag("PowerupRemover"))
        {
            buttery = false;
            doubleJump = false;
            Destroy(collision.gameObject);
            speed = 10;
        }
    }
    void WaitASecond()
    {
        canJump = true;
    }

    IEnumerator IncreasedGravity()
    {
        increasedGravity = true;
        if (increasedGravity)
            {
            gravForce += 0.1f;
            playerRB.AddForce(transform.up * gravForce * worldTime);
            if (gravForce > -2.5f)
                {
                    gravForce = -2.5f;
                }
            }

    }
    IEnumerator IncreaseFallSpeed()
    {
        yield return new WaitForSeconds(0.1f);
        
    }
    IEnumerator IncreaseAcceleration()
    {   
        yield return new WaitForSeconds(0.1f);
        currentAcceleration = currentAcceleration * 2f;
        if (currentAcceleration > 1000)
        {
            currentAcceleration = 998;
        }
    }
    IEnumerator RealTimeUpdate()
    {  
        yield return StartCoroutine("RealTimeUpdate");
    }
    
}
