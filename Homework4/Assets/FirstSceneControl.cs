using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControl
{

    void LoadResources();
}

public class FirstSceneControl : MonoBehaviour ,ISceneControl , IUserAction
{

    public CCActionManager actionManager { get; set; }
    public ScoreRecorder scoreRecorder { get; set; }
    public Queue<GameObject> diskQueue = new Queue<GameObject>();

    private int diskNumber;
    private int currentRound = -1;
    public int round = 3;
    private float time = 0;
    private GameState gameState = GameState.START;

    void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneControl = this;
        diskNumber = 10;
        this.gameObject.AddComponent<ScoreRecorder>();
        this.gameObject.AddComponent<DiskFactory>();
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        //director.currentSceneControl.LoadResources();
    }

    private void Update()
    {

        if (actionManager.DiskNumber == 0 && gameState == GameState.RUNNING)
        {
            gameState = GameState.ROUND_FINISH;

        }

        if (actionManager.DiskNumber == 0 && gameState == GameState.ROUND_START)
        {
            currentRound = (currentRound + 1) % round;
            NextRound();
            actionManager.DiskNumber = 10;
            gameState = GameState.RUNNING;
        }

        if (time > 1)
        {
            ThrowDisk();
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }


    }

    private void NextRound()
    {
        DiskFactory df = Singleton<DiskFactory>.Instance;
        for (int i = 0; i < diskNumber; i++)
        {
            diskQueue.Enqueue(df.GetDisk(currentRound));
        }

        actionManager.StartThrow(diskQueue);

    }

    void ThrowDisk()
    {
        if (diskQueue.Count != 0)
        {
            GameObject disk = diskQueue.Dequeue();

            /**
             * 以下几句代码是随机确定飞碟出现的位置
             */

            Vector3 position = new Vector3(0, 0, 0);
            float y = UnityEngine.Random.Range(0f, 4f);
            position = new Vector3(-disk.GetComponent<DiskObj>().direction.x * 7, y, 0);
            disk.transform.position = position;

            disk.SetActive(true);
        }

    }

    public void LoadResources()
    {
        //DiskFactory df = Singleton<DiskFactory>.Instance;
        //df.init(diskNumber);
        //GameObject greensward = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/greensward"));
    }


    public void GameOver()
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(700, 300, 400, 400), "GAMEOVER");

    }

    public int GetScore()
    {
        return scoreRecorder.score;
    }

    public GameState getGameState()
    {
        return gameState;
    }

    public void setGameState(GameState gs)
    {
        gameState = gs;
    }

    public void hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.GetComponent<DiskObj>() != null)
            {
                scoreRecorder.Record(hit.collider.gameObject);

                /**
                 * 如果飞碟被击中，那么就移到地面之下，由工厂负责回收
                 */

                hit.collider.gameObject.transform.position = new Vector3(0, -5, 0);
            }

        }
    }
}
