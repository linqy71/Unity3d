using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, PAUSE, START }

public interface IUserAction
{
    void GameOver();
    GameState getGameState();
    void setGameState(GameState gs);
    int GetScore();
    void hit(Vector3 pos);
}


public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    bool isFirst = true;
    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneControl as IUserAction;

    }


    private void OnGUI()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            Vector3 pos = Input.mousePosition;
            action.hit(pos);

        }

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 30;

        GUI.Label(new Rect(710, 5, 400, 400), "Score:" + action.GetScore().ToString(), myStyle);

        if (isFirst && GUI.Button(new Rect(700, 100, 90, 90), "Start"))
        {
            isFirst = false;
            action.setGameState(GameState.ROUND_START);
     
        }

        if (!isFirst && action.getGameState() == GameState.ROUND_FINISH && GUI.Button(new Rect(700, 100, 90, 90), "Next Round"))
        {
            action.setGameState(GameState.ROUND_START);
          
        }

    }


}
