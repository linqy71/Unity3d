using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------
// 此脚本加在text上
//----------------------------------

public class GameStatusText : MonoBehaviour {
    private int score = 0;
    private int textType;  //0为score，1为gameover

	void Start () {
        distinguishText();
	}
	
	void Update () {
		
	}

    void distinguishText() {
        if (gameObject.name.Contains("Score"))
            textType = 0;
        else
            textType = 1;
    }

    void OnEnable() {
        GameEventManager.myGameScoreAction += gameScore;
        GameEventManager.myGameOverAction += gameOver;
    }

    void OnDisable() {
        GameEventManager.myGameScoreAction -= gameScore;
        GameEventManager.myGameOverAction -= gameOver;
    }

    void gameScore() {
        if (textType == 0) {
            score++;
            this.gameObject.GetComponent<Text>().text = "Score: " + score;
        }
    } 

    void gameOver() {
        if (textType == 1)
            this.gameObject.GetComponent<Text>().text = "Game Over!";
    }
}
