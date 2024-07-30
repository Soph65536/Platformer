using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Cutscenes : MonoBehaviour
{
    public UnityEvent CutsceneEvent;

    //position of the current cutscene playing in gamemanager's viewedcutscenes array
    private int CurrentCutscene;

    //create object and its animator references
    private GameObject PlayerObject;
    private Animator PlayerAnimator;

    private void Awake()
    {
        //give currentcutscene a value
        CurrentCutscene = 0;

        //set object and then animator references
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerAnimator = PlayerObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check for player collision
        if(collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            //run event
            CutsceneEvent.Invoke();
        }
    }

    //wait for end of cutscene delay coroutine
    public IEnumerator EndOfCutscene(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        GameManager.Instance.inCutscene = false;
        Destroy(gameObject);
    }

    public void PlayerAwaken()
    {
        GameManager.Instance.inCutscene = true;

        //set currentcutscene for this cutscene
        CurrentCutscene = 0;

        PlayerAnimator.SetTrigger("AwakenCutscene");

        StartCoroutine("EndOfCutscene", 2.5f);
    }
}
