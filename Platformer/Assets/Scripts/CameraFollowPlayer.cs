using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public float followspeed = 2;
    public GameObject followobject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 followposition = this.transform.position;
        followposition.x = Mathf.Lerp(this.transform.position.x, followobject.transform.position.x, followspeed * Time.deltaTime);
        followposition.y = Mathf.Lerp(this.transform.position.y, followobject.transform.position.y, followspeed * Time.deltaTime);
        this.transform.position = followposition;
    }
}
