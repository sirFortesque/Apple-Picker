using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Apple : MonoBehaviour {
   
    public static float bottomY = -20f;

	void Update () {
        if (ApplePicker.instance.allowMove) {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        } else {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }     

        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);          
            ApplePicker.instance.AppleDestroyed();
        }
	}
}
