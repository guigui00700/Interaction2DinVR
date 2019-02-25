using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationScreen : MonoBehaviour {

    // Use this for initialization
    public float RotationSpeed;
    public string RotationKey;
    bool rotationOn;
    public Transform from;
    private Transform to;


    void Start () {
        rotationOn = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(RotationKey) ) rotationOn = !rotationOn;
        if (rotationOn)
        {
            //transform.Rotate(0, RotationSpeed *Time.deltaTime, 0);
            //Debug.Log("r");
            to = from;
            to.Rotate(0, RotationSpeed*Time.deltaTime, 0);
            //to.rotation = new Quaternion(from.rotation.x, from.rotation.y + 15, from.rotation.z, from.rotation.w);
            //Debug.Log(to.rotation.y);
            transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * RotationSpeed);
        }
	}
}
