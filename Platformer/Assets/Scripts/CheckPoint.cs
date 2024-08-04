using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if(collision.gameObject.GetComponent<PlayerMovement>().grounded) { GameManager.Instance.SpawnPosition = collision.gameObject.transform.position; }
        }
    }
}
