using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour {

    public Button Quit;
    // Use this for initialization

    void QuitApplication()
    {
        Application.Quit();
        
    }

    void Start () {
        Quit.onClick.AddListener(QuitApplication);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
