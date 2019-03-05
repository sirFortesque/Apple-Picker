using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class ApplePicker : MonoBehaviour {

    public static ApplePicker      instance;

    [Header("Set in Inspector")]
    public GameObject              appleTreePrefab;
    public GameObject              basketPrefab;
    public static int              numBaskets = 3;
    public float                   basketBottomY = -14f;
    public float                   basketSpacingY = 2f;
    public GameObject              faderObj;
    public Image                   faderImg;
    public bool                    gameOver = false;
    public float                   fadeSpeed = .02f;
    public int                     levelNumber;
    public int                     startGoal;
    public int                     goal;

    [Header("Set Dynamically")]
    public static List<GameObject> basketList;
    public Vector3                 grv = Vector3.zero; // Gravity variable
    public bool                    allowMove;
    private Color                  fadeTransparency = new Color(0, 0, 0, .04f);
    private string                 currentScene;
    private AsyncOperation         async;


    void Awake() {
        // Only 1 Game Manager can exist at a time
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = GetComponent<ApplePicker>();
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            levelNumber = 0;
            goal = 0;
        } else {
            Destroy(gameObject);
        }
    }

	void Start () {
        grv =  Physics.gravity;        
	}

    public void AppleDestroyed()
    {
        // Destroy all of the falling apples
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }
        
        // Destroy one of the baskets
        // Get the index of the last Basket in basketList
        int basketIndex = basketList.Count - 1;
        numBaskets -= 1;

        // Get a reference to that Basket GameObject
        GameObject tBasketGO = basketList[basketIndex];

        // Remove the Basket from the list and destroy the GameObject
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);

        // If there are no Baskets left, restart the game
        if (basketList.Count == 0)
        {
            Time.timeScale = 0;
            GFXmanager.instance.loseScreen.SetActive(true);
            ApplePicker.instance.gameOver = true;           

            if (GFXmanager.instance.score > PlayerPrefs.GetInt("HighScore")) {
                PlayerPrefs.SetInt("HighScore", GFXmanager.instance.score);
                GFXmanager.instance.highScoreLoseTxt.text = "New Best: " + PlayerPrefs.GetInt("HighScore").ToString();
            } else {
                GFXmanager.instance.highScoreLoseTxt.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
            }

            GFXmanager.instance.scoreLoseTxt.text = GFXmanager.instance.score.ToString();
        }
    } 

    public void PlayButton() {

        GFXmanager.instance.startScreen.SetActive(false);

        // Instantiating AppleTree
        GameObject tAppleTreeGO = Instantiate<GameObject>(appleTreePrefab);
        Vector3 pos = Vector3.zero;
        pos.y = 12;
        pos.z = 80;
        tAppleTreeGO.transform.position = pos;
        
            // Instantiating baskets
            basketList = new List<GameObject>();
            for (int i = 0; i < numBaskets; i++) {
                GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
                //Vector3 pos = Vector3.zero;
                pos.y = basketBottomY + (basketSpacingY * i);
                pos.z = 80;
                tBasketGO.transform.position = pos;
                basketList.Add(tBasketGO);
            }
                
    }

    //////////////////////////////////////////////////////////////////////

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        Time.timeScale = 1;        
        currentScene = scene.name;

        // increasing level difficulty
        if (CurrentSceneName == "Scene_Game") {
            allowMove = true;      
     
            levelNumber++;
            goal = startGoal * levelNumber;
            GFXmanager.instance.Score = 0;       
            grv.y -= 15;
            Physics.gravity = grv;
         
            Debug.Log("Gravity - " + Physics.gravity.y);
            Debug.Log("level - " + levelNumber);
            Debug.Log("goal - " + goal);        
        }
        
        if (CurrentSceneName == "Scene_Menu") {           
            numBaskets = 3;            
            grv.y = -25;
            Physics.gravity = grv;           
        }        

        instance.StartCoroutine(FadeIn(instance.faderObj, instance.faderImg));
    }

    // Iterate the fader transparency to 0%
    public IEnumerator FadeIn(GameObject faderObject, Image fader) {
        while (fader.color.a > 0) {
            fader.color -= fadeTransparency;
            yield return new WaitForSeconds(fadeSpeed);
        }
        faderObject.SetActive(false);
    }

    //Iterate the fader transparency to 100%
    IEnumerator FadeOut(GameObject faderObject, Image fader) {
        faderObject.SetActive(true);      
        while (fader.color.a < 1) {
            fader.color += fadeTransparency;
            yield return new WaitForSeconds(fadeSpeed);
        }
        ActivateScene(); //Activate the scene when the fade ends
    }

    // Allows the scene to change once it is loaded
    public void ActivateScene() {
        async.allowSceneActivation = true;
    }

    private bool isReturning = false;
    public void ReturnToMenu() {
        if (isReturning) {
            return;
        }

        if (CurrentSceneName != "Scene_Menu") {
            Time.timeScale = 1;
            ApplePicker.instance.allowMove = false;
            StopAllCoroutines();           
            LoadScene("Scene_Menu");
            ApplePicker.instance.levelNumber = 0;
            ApplePicker.instance.goal = 0;
            isReturning = true;
        }
    }

    // Get the current scene name
    public string CurrentSceneName {
        get {
            return currentScene;
        }
    }

    // Load a scene with a specified string name
    public void LoadScene(string sceneName) {        
        instance.StartCoroutine(Load(sceneName));
        instance.StartCoroutine(FadeOut(instance.faderObj, instance.faderImg));
    }

    // Begin loading a scene with a specified string asynchronously
    IEnumerator Load(string sceneName) {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return async;
        isReturning = false;
    }

    // Reload the current scene
    public void ReloadScene() {
        StopAllCoroutines();
        allowMove = false;
        Time.timeScale = 1;      
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame() {
        // If we are running in a standalone build of the game
        #if UNITY_STANDALONE
            // Quit the application
            Application.Quit();
        #endif

        // If we are running in the editor
        #if UNITY_EDITOR
            // Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif 
    }

}
