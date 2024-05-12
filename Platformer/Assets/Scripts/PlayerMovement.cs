using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //declare constants!!!

    //movement speeds
    const float airmovespeed = 5f;
    const float watermovespeed = 3f;

    const float airjumpspeed = 10f;

    const float airdrag = 0f;
    const float waterdrag = 3f;


    //declare variables!!!

    //game menu object
    [SerializeField] private GameObject GameButtons;

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
        if(Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.GameMenuOpen)
        {
            //set game menu to open and create game menu
            GameManager.Instance.GameMenuOpen = true;
            Instantiate(GameButtons, Vector3.zero, Quaternion.identity);
        }

        //movement
        if (Input.GetKey(KeyCode.Space) && grounded && (rb.velocity.y > -00.1 && rb.velocity.y < 0.01))
        {
            //jump
            rb.velocity=new Vector3(0, jumpforce, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //moveleft
            transform.position += Vector3.left * moveforce * Time.deltaTime;
            
            //animateleft
            animationhorizontal=0;
            animationmoving=1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //moveright
            transform.position += Vector3.right * moveforce * Time.deltaTime;

            //animateright
            animationhorizontal=1;
            animationmoving=1;
        }

        //check if stop moving
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            //moving is false if no keys pressed
            animationmoving=0;
        }

        //update animator
        animator.SetFloat("horizontal", animationhorizontal);
        animator.SetFloat("moving", animationmoving);
        animator.SetFloat("airborne", animationairborne);
    }


    //kill player subroutine
    public void KillPlayer()
    {
        transform.position = GameManager.Instance.SpawnPosition;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collided with ground or checkpoint and not falling then grounded is true
        //else is false
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Checkpoint")
        {
            grounded=true;
            animationairborne=0;
            Debug.Log("Grounded");
        }
           
        //if collided with enemy go to startingpos
        if(collision.gameObject.tag == "Enemy")
        {
            KillPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //reset grounded since not colliding with anything
        grounded=false;
        animationairborne=1;
        Debug.Log("Not Grounded");
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if collided with water then slow movement
        if(collider.gameObject.layer == 4)
        {
            rb.drag = waterdrag;
            moveforce = watermovespeed;
            Debug.Log("Water");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //reset drag and movement values since no longer in water
        rb.drag = airdrag;
        moveforce = airmovespeed;
    }
}
