using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {

    public static AppleTree instance;

    [Header("Set in Inspector")]
    // Prefab for instantiating apples
    public GameObject   applePrefab;
    
    // Speed at which the AppleTree moves
    public float        speed = 8f; 

    // Distance where AppleTree turns around
    public float        leftAndRightEdge = 20f;

    // Chance that the AppleTree will change directions
    public float        chanceToChangeDirections = 0.02f;

    //The number of seconds since AppleTree can change directions
    public float        secondsNeedToChangeDirections = 3f;

    //The number of seconds required to increase the difficulty
    public float        secondsNeedToIncreseDifficulty = 2f;

    // Rate at which Apples will be instantiated
    public float        secondsBetweenAppleDrops = 1f;

    // difficulty level of the game
    //public List<int>  dif = new List<int>();

    //Time at which was changed direction
    //private float directionChanged = 0;


    // Use this for initialization
    void Start () {      
        instance = GetComponent<AppleTree>();
		//Dropping apples every second
        Invoke("DropApple", 2f);
	}

    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }
	
	// Update is called once per frame
	void Update () {
        if (ApplePicker.instance.allowMove) {
            // Basic Movement
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;

            // Changing Direction
            if (pos.x < -leftAndRightEdge) {
                speed = Mathf.Abs(speed);
               //directionChanged = Time.time;
            } else if (pos.x > leftAndRightEdge) {
                speed = -Mathf.Abs(speed);
                //directionChanged = Time.time;
            }   
        }        
	}

    void FixedUpdate()
    {

        /*
        // Changing difficulty depending on time
        if ((Time.time % secondsNeedToIncreseDifficulty) == 0)
        {
            //Debug.Log("YAP!");
            speed += (speed>0) ? 1 : -1;
            chanceToChangeDirections += 0.01f;
        }         
        Whether the necessary amount of time 
        has passed since the last changing of direction
        if ((Time.time - directionChanged) >= secondsNeedToChangeDirections) { 
        */


        // Changing direction randomly
            if (Random.value < chanceToChangeDirections)
            {
                speed *= -1;
                //directionChanged = Time.time;
            }        
    }
}
