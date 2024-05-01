using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPlayerCollision : MonoBehaviour
{
    public Renderer rend;

    void Start()
    {
        rend=GetComponent<Renderer>();
        rend.enabled=false;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        //make text object visible if player has touched it

        if(collision.gameObject.tag=="Player")
        {
            rend.enabled=true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //make text object invisible once player stops touching it

        if(collision.gameObject.tag=="Player")
        {
            rend.enabled=false;
        }
    }

}
