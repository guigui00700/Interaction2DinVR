using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penduleScript : MonoBehaviour {


    public Animation animations;
    public float penduleSpeed;
    public string penduleKey;
    bool penduleOn;//,positifX;
    //public Transform from;
   // private Transform to;
   // float maxPositionX, minPositionX,originePositionX;


    // Use this for initialization
    void Start () {
        /*penduleOn = false;
        positifX = true;
        originePositionX = transform.position.x;
        maxPositionX = transform.position.x + 1f;
        minPositionX = transform.position.x - 1f;
        */

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(penduleKey)) penduleOn = !penduleOn;
        if (penduleOn)
        {
            /*if (positifX && transform.position.x >= originePositionX) 
            {
                transform.Translate(penduleSpeed * Time.deltaTime, 0.5f * penduleSpeed * Time.deltaTime, 0);
                if (transform.position.x >= maxPositionX) positifX = false;
            }
            else if( positifX && transform.position.x < originePositionX)
            {
                transform.Translate(penduleSpeed * Time.deltaTime, -0.5f * penduleSpeed * Time.deltaTime, 0);
                if (transform.position.x <= minPositionX) positifX = true;
            }else if(!positifX && transform.position.x >= originePositionX)
            {
                transform.Translate(-penduleSpeed * Time.deltaTime, -0.5f * penduleSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.Translate(-penduleSpeed * Time.deltaTime, 0.5f * penduleSpeed * Time.deltaTime, 0);
            }

            if (transform.position.x >= maxPositionX) positifX = false;
            if (transform.position.x <= minPositionX) positifX = true;*/
            animations["pendule"].speed = penduleSpeed;
            animations.Play("pendule");
        }
    }
}
