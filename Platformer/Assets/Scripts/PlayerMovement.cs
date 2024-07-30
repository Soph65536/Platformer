using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //declare constants!!!

    //animation delays
    const float deathdelay = 1f;
    const float respawndelay = 1.75f;

    //movement speeds
    const float airmovespeed = 5f;
    const float watermovespeed = 3f;

    const float airjumpspeed = 10f;

    const float airdrag = 0f;
    const float waterdrag = 3f;


    //declare variables!!!

    //game menu object
    [SerializeField] private GameObject GameButtons;

    //player death delay bool to prevent instant respawn and only play death one
    private bool isDead;

    //movement
    public float moveforce;
    public float jumpforce;
    public bool grounded;

    //animation
    public float animationhorizontal;
    public float animationmoving;
    public float animationairborne;
    Animator animator;

    //rigidbody
    Rigidbody2D rb;


    void Awake()
    {
        //define animator
        animator=GetComponent<Animator>();
        //define rigidbody
        rb = transform.GetComponent<Rigidbody2D>();

        //setting initial variables
        isDead = false;

        moveforce = airmovespeed;
        jumpforce = airjumpspeed;
        grounded = false;

        animationhorizontal = 0f;
        animationmoving = 0f;
        animationairborne = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        //open menu
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.GameMenuOpen)
        {
            //set game menu to open and create game menu
            GameManager.Instance.GameMenuOpen = true;
            Instantiate(GameButtons, Vector3.zero, Quaternion.identity);
        }


        //only allow player to be controlled when not in a cutscene or dead
        if (!isDead && !GameManager.Instance.inCutscene)
        {
            //movement
            if (Input.GetKey(KeyCode.Space) && grounded)
            {
                //jump
                rb.velocity = new Vector3(0, jumpforce, 0);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                //moveleft
                transform.position += Vector3.left * moveforce * Time.deltaTime;

                //animateleft
                animationhorizontal = 0;
                animationmoving = 1;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                //moveright
                transform.position += Vector3.right * moveforce * Time.deltaTime;

                //animateright
                animationhorizontal = 1;
                animationmoving = 1;
            }


            //check if stop moving
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)
                || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                //moving is false if no keys pressed
                animationmoving = 0;
            }


            //update animator
            animator.SetFloat("horizontal", animationhorizontal);
            animator.SetFloat("moving", animationmoving);
            animator.SetFloat("airborne", animationairborne);
        }
    }


    //kill player coroutine
    public IEnumerator KillPlayer()
    {
        isDead = true;

        //play death animation and wait for player death delay
        animator.SetTrigger("PlayerDeath");
        yield return new WaitForSeconds(deathdelay);

        //respawn player and wait for respawn animation
        transform.position = GameManager.Instance.SpawnPosition;
        animator.SetTrigger("PlayerRespawn");
        yield return new WaitForSeconds(respawndelay);

        isDead = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collided with ground or checkpoint and not falling then grounded is true
        //else is false
        if((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Checkpoint") 
            && (rb.velocity.y > -00.1 && rb.velocity.y < 0.01))
        {
            grounded=true;
            animationairborne=0;
        }
           
        //if collided with enemy go to startingpos
        if(collision.gameObject.tag == "Enemy" && !isDead)
        {
            StartCoroutine("KillPlayer");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //reset grounded since not colliding with anything
        grounded=false;
        animationairborne=1;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if collided with water then slow movement
        if(collider.gameObject.layer == 4)
        {
            rb.drag = waterdrag;
            moveforce = watermovespeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //reset drag and movement values since no longer in water
        rb.drag = airdrag;
        moveforce = airmovespeed;
    }
}
