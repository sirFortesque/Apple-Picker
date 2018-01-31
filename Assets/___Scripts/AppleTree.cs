using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {
    [Header("Set in Inspector")]

    // Prefab for instantiating apples
    public GameObject applePrefab;
    // Speed at which the AppleTree moves
    public float speed = 8; 
    // Distance where AppleTree turns around
    public float leftAndRightEdge = 20f;
    // Chance that the AppleTree will change directions
    public float chanceToChangeDirections = 0.02f;
    //Number of seconds since AppleTree can change directions
    public float secondsNeedForChangeDirections = 3f;
    // Rate at which Apples will be instantiated
    public float secondsBetweenAppleDrops = 1f;
    // difficulty level of the game
    //public List<int> dif = new List<int>();


    //Time at which was changed direction
    private float directionChanged = 0;


    // Use this for initialization
    void Start () {
        //dif.Add(0);
        //speed.Add(10f);
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
        // Changing difficulty depending on time



        // Basic Movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos; 
       
        // Changing Direction
        if ( pos.x < -leftAndRightEdge )
        {
            speed = Mathf.Abs(speed);
            directionChanged = Time.time;
        } else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);
            directionChanged = Time.time;
        }
	}

    void FixedUpdate()
    {
        //Whether the necessary amount of time 
        //has passed since the last changing of direction
        if ((Time.time - directionChanged) >= secondsNeedForChangeDirections) { 
            // Changing direction randomly
            if (Random.value < chanceToChangeDirections)
            {
                speed *= -1;
                directionChanged = Time.time;
            }
        }
    }
}
