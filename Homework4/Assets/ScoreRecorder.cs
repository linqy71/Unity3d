using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{

    /**
     * score是玩家得到的总分
     */

    public int score;

    /**
     * scoreTable是一个得分的规则表，每种飞碟的颜色对应着一个分数
     */

    private Dictionary<Color, int> scoreTable = new Dictionary<Color, int>();

    // Use this for initialization
    void Start()
    {
        score = 0;
        scoreTable.Add(Color.yellow, 10);
        scoreTable.Add(Color.red, 20);
        scoreTable.Add(Color.green, 40);
    }

    public void Record(GameObject disk)
    {
        score += scoreTable[disk.GetComponent<DiskObj>().color];
    }

    public void Reset()
    {
        score = 0;
    }
}
