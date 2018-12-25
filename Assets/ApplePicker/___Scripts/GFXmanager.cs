using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GFXmanager : MonoBehaviour {

    public static GFXmanager instance;

    [Header("Set in Inspector:")]    
    public Text                 scoreTxt;
    [Header("\n")]

    [Header("*START SCREEN*")]
    public GameObject           startScreen;
    public Text                 levelNumberTxt;
    public Text                 goalTxt;
    [Header("\n")]

    [Header("*PAUSE SCREEN*")]
    public  GameObject          pauseScreen;
    public  Text                levelNumberPauseScreenTxt;
    public  Text                goalPauseScreenTxt;
    public  Text                highScorePauseScreenTxt;
    private static bool         paused;
    [Header("\n")]

    [Header("*WIN SCREEN*")]    
    public  GameObject          winScreen;
    public  Text                scoreWinTxt;
    public  Text                highScoreWinTxt;
    [Header("\n")]

    [Header("*LOSE SCREEN*")]    
    public  GameObject          loseScreen;
    public  Text                scoreLoseTxt;
    public  Text                highScoreLoseTxt;
    [Header("\n")]

    [Header("Set Dynamically:")]
    public int                  score;

    void Awake() {        
        instance = GetComponent<GFXmanager>();
        paused = false;  
    }

    void Start() {
        levelNumberTxt.text = "Level " + ApplePicker.instance.levelNumber.ToString() + " !";
        levelNumberPauseScreenTxt.text = levelNumberTxt.text;
        goalTxt.text = "Your goal \n" + ApplePicker.instance.goal.ToString() + " points";
        goalPauseScreenTxt.text = "Goal " + ApplePicker.instance.goal.ToString();
        highScorePauseScreenTxt.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMethod();
        }
    }

    public int Score {
        get {
            return score;            
        }
        set {
            score = value;
            if (score >= ApplePicker.instance.goal) {   // WIN!!!            
                ApplePicker.instance.allowMove = false;
                winScreen.SetActive(true);
                Time.timeScale = 0;

                if (score > PlayerPrefs.GetInt("HighScore")) {
                    PlayerPrefs.SetInt("HighScore", score);
                    highScoreWinTxt.text = "New Best: " + PlayerPrefs.GetInt("HighScore").ToString();
                } else {
                    highScoreWinTxt.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
                }

                scoreWinTxt.text = score.ToString();
            }             
            scoreTxt.text = score.ToString();
        }
    }

    /*
	// Show the game over panel
	public void GameOver() {
        StopAllCoroutines();

        if (score >= ApplePicker.instance.goal) {            
	        winScreen.SetActive(true);
            /*
            SFXManager.instance.PauseSFX(Clip.Hyperfun);
            SFXManager.instance.StopSFX(Clip.X2Score);
            SFXManager.instance.StopSFX(Clip.Clock);
	        SFXManager.instance.PlaySFX(Clip.Win);
             

	        if (score > PlayerPrefs.GetInt("HighScore")) {
	            PlayerPrefs.SetInt("HighScore", score);
	            highScoreWinTxt.text = "New Best: " + PlayerPrefs.GetInt("HighScore").ToString();
	        } else {
	            highScoreWinTxt.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
	        }

	        scoreWinTxt.text = score.ToString();
	        return;
	    }
        
	    if (moveCounter <= 0) {
            loseScreen.SetActive(true);
            ApplePicker.instance.gameOver = true;

            //ApplePicker.instance.StopSFX();
	        //SFXManager.instance.PlaySFX(Clip.Lose);

	        if (score > PlayerPrefs.GetInt("HighScore")) {
	            PlayerPrefs.SetInt("HighScore", score);
	            highScoreLoseTxt.text = "New Best: " + PlayerPrefs.GetInt("HighScore").ToString();
	        } else {
	            highScoreLoseTxt.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
	        }

	        scoreLoseTxt.text = score.ToString();
	    }	
         
	}
    */

    public void PauseMethod() {
        if (!winScreen.activeInHierarchy && !loseScreen.activeInHierarchy && !startScreen.activeInHierarchy) {
        if (!paused) {
            ApplePicker.instance.allowMove = false;
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            paused = true;            
        } else {
            ApplePicker.instance.allowMove = true;
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            paused = false;
        }
        }
    }

}