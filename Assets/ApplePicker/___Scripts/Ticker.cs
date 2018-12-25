using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticker : MonoBehaviour {

    [Header("Set in Inspector:")]
    public Text     highScore;
    public Canvas   canvas;
    public int      speed;

    [Header("Set Dynamically")]
    Vector3         initialPosition;
    float           canvasWidth;

    void Start() {
        if(PlayerPrefs.HasKey("HighScore")){
            highScore.text = "Best result " + PlayerPrefs.GetInt("HighScore").ToString();
        }   
     
        initialPosition = transform.localPosition;
        canvasWidth = canvas.GetComponent<Transform>().position.x*2;
    }

	// Update is called once per frame
	void Update () {        
        // If Ticker go out of the boundaries set its position to the initial state
        if (transform.position.x >= (initialPosition.x*2 + canvasWidth)) {            
            transform.localPosition = initialPosition;
        } 
        
        // Basic movement relativ to localPosition
        transform.Translate(Vector3.right * speed * Time.deltaTime);        
	}
}
