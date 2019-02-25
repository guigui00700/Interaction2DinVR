using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScale : MonoBehaviour {
    public Button BIncreaseArea;
    public Button BDecreaseArea;
    
	// Use this for initialization
	void Start () {
        BIncreaseArea.onClick.AddListener(IncreaseArea);
        BDecreaseArea.onClick.AddListener(DecreaseArea);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Plus) && Input.GetKey(KeyCode.LeftControl)) {
            IncreaseArea();
        }
        else if (Input.GetKeyDown(KeyCode.Minus) && Input.GetKey(KeyCode.LeftControl)){
            DecreaseArea();
        }
    }

    void IncreaseArea()
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;
        transform.localScale = new Vector3((float)(x*1.1), (float)(y*1.1), (float)(z*1.1));
    }

    void DecreaseArea()
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;
        transform.localScale = new Vector3((float)(x*0.9), (float)(y*0.9), (float)(z*0.9));
    }
}
