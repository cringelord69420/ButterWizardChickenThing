using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public GameObject gmScreen;
    public GameObject particleEmitter;

    
    
    private float worldTime = 1; //MULTIPLY THIS WITH TIMEDELTATIME ALWAYS
    private float jumpForce = 400f;
    private float speed = 10f;
    private float currentAcceleration = 1.5f;
    public float gravForce = 120f;
    public float jumpPadForce = 600f;
    public float setNegative = 1;
    public float emitterStayTime = 0.5f;

    public Rigidbody2D playerRB;
    
    public bool gameOver;
    public bool isOnGround;
    public bool doubleJump;
    public bool buttery;
    public bool canJump;
    public bool increasedGravity;

    public Sprite normal, buttered, winged, super;
    //visible is the game over texture(which is always following the player), and invisible is a nothing texture
    public Sprite visible, invisible;
    // Start is called before the first frame update

    public ParticleSystem doubleJumpParticles;
    public ParticleSystem butterGetParticles;
    public ParticleSystem butterParticles;
    public ParticleSystem jumpParticles;
    public ParticleSystem particleRemove;
    void Start()
    {
        isOnGround = true;
        gameOver = false;
        //instantiates the rigidbody component of the player for use in code
        playerRB = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = normal;
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
        
        //Basic movement

        //timedeltatime is multiplied with worldtime first because if its not, getaxishorizontal will be reversed if it happens to be negative
        transform.Translate(Vector3.right * speed * (Time.deltaTime * worldTime) * (setNegative * Input.GetAxis("Horizontal")));

        //if the game isnt over, it checks for player input/moves the player
        if (!gameOver)
        {
            if (Input.GetKeyDown("a"))
            {
                //flips the particle emitter when not active
                if (particleEmitter.GetComponent<ParticleEmitterPlayerFollow>().notBeingUsed){
                    particleEmitter.transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                transform.localEulerAngles = new Vector3(0, 180, 0);
                setNegative = -1;
                if (isOnGround)
                {
                jumpParticles.Play();
                }
                
            }
            if (Input.GetKeyDown("d"))
            {
                if(particleEmitter.GetComponent<ParticleEmitterPlayerFollow>().notBeingUsed){
                    particleEmitter.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                transform.localEulerAngles = new Vector3(0, 0, 0);
                setNegative = 1;
                if (isOnGround)
                {
                jumpParticles.Play();
                }
            }
            if (buttery)
            {

                if (Input.GetKey("d"))
                {

                    playerRB.AddForce(transform.right * (currentAcceleration * worldTime));

                    //waits for a sec before accelerating, this is to prevent acceleration from scaling with framerate
                    Invoke("IncreaseAcceleration", 0.1f);
                    butterParticles.Play();
                }
                if (Input.GetKey("a"))
                {

                    playerRB.AddForce(transform.right * (currentAcceleration * worldTime));

                    //waits for a sec before accelerating, this is to prevent acceleration from scaling with framerate
                    Invoke("IncreaseAcceleration", 0.1f);
                    butterParticles.Play();
                }
            }
            
            

            
            if (Input.GetKeyDown("space"))
            {
                //seperated the if statements for preformance reasons
                if (isOnGround)
                {
                playerRB.AddForce(transform.up * jumpForce * worldTime);
                isOnGround = false;
                increasedGravity = false;
                Invoke("WaitASecond", 0.3f);
                    jumpParticles.Play();
                    particleEmitter.GetComponent<ParticleEmitterPlayerFollow>().notBeingUsed = false;
                    Invoke("SetBackToTrue", emitterStayTime);

                }
                if (doubleJump && canJump)
                {
                playerRB.AddForce(transform.up * (jumpForce * 1.5f) * worldTime);
                canJump = false;
                InvokeRepeating("IncreasedGravity", 0.4f, 0.2f);
                increasedGravity = true;
                doubleJumpParticles.Play();
                particleEmitter.GetComponent<ParticleEmitterPlayerFollow>().notBeingUsed = false;
                Invoke("SetBackToTrue", 0.6f);


                }
                
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the object is ground they can jump off of
        if (collision.gameObject.CompareTag("Ground"))
        {
            increasedGravity = false;
            isOnGround = true;
            canJump = false;
            CancelInvoke("IncreasedGravity");
            gravForce = 120;
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
            GetComponent<SpriteRenderer>().sprite = winged;
        }
        if (collision.gameObject.CompareTag("Buttery"))//if they touch a double jump powerup
        {
            buttery = true;
            doubleJump = false;
            //destroys the object with the tag double jump
            Destroy(collision.gameObject);
            speed = 0;
            GetComponent<SpriteRenderer>().sprite = buttered;
        }
        if (collision.gameObject.CompareTag("PowerupRemover"))
        {
            particleRemove.Play();
            buttery = false;
            doubleJump = false;
            Destroy(collision.gameObject);
            speed = 10;
            GetComponent<SpriteRenderer>().sprite = normal;
        }
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            if (increasedGravity)
            {
                jumpPadForce = 600f + (gravForce * 1.5f);
                if (jumpPadForce > 1500f)
                {
                    jumpPadForce = 1500f;
                }
                
            }
            playerRB.AddForce(transform.up * worldTime * jumpPadForce);
            increasedGravity = false;
            
            
            CancelInvoke("IncreasedGravity");
            gravForce = 120f;
            jumpPadForce = 600f;
            isOnGround = false;
            
        }
    }
    
    void WaitASecond()
    {
        canJump = true;
    }

    void IncreasedGravity()
    {
        increasedGravity = true;
        if (increasedGravity)
        {
            gravForce += 20f;
            playerRB.AddForce(transform.up * -gravForce * worldTime);
            if (gravForce > 300f)
            {
                gravForce = 300f;
            }
        }

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
    void GameStart()
    {
        worldTime = 1;
    }
    void SetBackToTrue()
    {
        particleEmitter.GetComponent<ParticleEmitterPlayerFollow>().notBeingUsed = true;
    }
    
}
