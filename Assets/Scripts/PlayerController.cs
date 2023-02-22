using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public float gravityModifier = 0;
    public bool gameOver;
    public Rigidbody2D playerRB;
    public float jumpForce = 400;
    public bool isOnGround;
    // Start is called before the first frame update
    void Start()
    {
        isOnGround = true;
        gameOver = false;
        //instantiates the rigidbody component of the player for use in code
        playerRB = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //if the game isnt over, it checks for player input/moves the player
        if (!gameOver)
        {

            //this is getting reworked into a rigidbody system soon
            transform.Translate(Vector3.right * speed * Time.deltaTime * Input.GetAxis("Horizontal"));
            if (Input.GetKeyDown("space") && isOnGround)
            {
                playerRB.AddForce(transform.up * jumpForce);
                isOnGround = false;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;

        }
    }
}