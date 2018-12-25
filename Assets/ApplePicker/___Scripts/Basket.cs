using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour {
   	
	// Update is called once per frame
	void Update () {
        if (ApplePicker.instance.allowMove) {
            // Get the current screen position of the mouse from Input
            Vector3 mousePos2D = Input.mousePosition;

            // The Camera's z position sets how far to push the mouse into 3D
            mousePos2D.z = -Camera.main.transform.position.z;

            // Convert the point from 2D screen space into 3D game world space
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

            // Move the x position of this Basket to the x position of the Mouse
            Vector3 pos = this.transform.position;
            pos.x = mousePos3D.x;
            this.transform.position = pos;    
        }		
	}

    void OnCollisionEnter(Collision coll) {
        // Find out what hit this basket
        GameObject collidedWith = coll.gameObject;        
        if (collidedWith.tag == "Apple") {
            Destroy(collidedWith);
           
            // Add points for catching the apple
            GFXmanager.instance.Score += 100;            
        }
    }
}
