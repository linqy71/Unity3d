using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public interface UserActions
{
    void priestSOnB();
    void priestEOnB();
    void devilSOnB();
    void devilEOnB();
    void moveBoat();
    void offBoatL();
    void offBoatR();
    void restart();
}
// 获取当前游戏对象的运动状态
public interface QueryGameStatus
{
    bool isMoving();
    void setMoving(bool state);
    string getMessage();
    void setMessage(string message);
}

// 游戏场景控制，负责创造实例，关联游戏对象，定义动作等
public class GameSceneController : System.Object, UserActions, QueryGameStatus
{
    private static GameSceneController _instance;
   // private BaseCode _base_code;
    private GenGameObject _gen_game_obj;
    private bool moving = false;
    private string message = "";
    public string gameRule = "Boat capacity is 2. One person must steer the boat from one side to the other side. " +
        "Click 'on' buttons to move a person onto the boat and click the 'go' buttons to move the boat to the other side. " +
        "If priests are outnumbered by devils on either side, they get killed by devils!    Sphere---Priest Cube---Devil";

    public static GameSceneController GetInstance()
    {
        if (null == _instance) _instance = new GameSceneController();
        return _instance;
    }

   // public BaseCode getBaseCode() { return _base_code; }
    //internal void setBaseCode(BaseCode bc) { if (null == _base_code) _base_code = bc; }

    public GenGameObject getGenGameObject() { return _gen_game_obj; }
    internal void setGenGameObject(GenGameObject ggo) { if (null == _gen_game_obj) _gen_game_obj = ggo; }

    public bool isMoving() { return moving; }
    public void setMoving(bool state) { this.moving = state; }
    public string getMessage() { return message; }
    public void setMessage(string message) { this.message = message; }

    public void priestSOnB() { _gen_game_obj.priestStartOnBoat(); }
    public void priestEOnB() { _gen_game_obj.priestEndOnBoat(); }
    public void devilSOnB() { _gen_game_obj.devilStartOnBoat(); }
    public void devilEOnB() { _gen_game_obj.devilEndOnBoat(); }
    public void moveBoat() { _gen_game_obj.moveBoat(); }
    public void offBoatL() { _gen_game_obj.getOffTheBoat(0); }
    public void offBoatR() { _gen_game_obj.getOffTheBoat(1); }

    public void restart()
    {
        moving = false;
        message = "";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
