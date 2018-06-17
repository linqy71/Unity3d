using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class GenGameObject : MonoBehaviour
{
    Stack<GameObject> priests_start = new Stack<GameObject>();
    Stack<GameObject> priests_end = new Stack<GameObject>();
    Stack<GameObject> devils_start = new Stack<GameObject>();
    Stack<GameObject> devils_end = new Stack<GameObject>();
    // 为船设置2个位子，同时设定船的移动速度，岸上或船上牧师或者魔鬼的间距
    GameObject[] boat = new GameObject[2];
    GameObject boat_obj;
    int side = 1;
    public float speed = 6f;
    float gap = 0.7f;
    // 预设了游戏中所有对象的所处位置
    Vector3 shoreStartPos = new Vector3(-6, 0, 0);
    Vector3 shoreEndPos = new Vector3(6, 0, 0);
    Vector3 boatStartPos = new Vector3(-3, 0, 0);
    Vector3 boatEndPos = new Vector3(3, 0, 0);
    Vector3 priestStartPos = new Vector3(-6, 1.5f, 0);
    Vector3 priestEndPos = new Vector3(4, 1.5f, 0);
    Vector3 devilStartPos = new Vector3(-8, 1.5f, 0);
    Vector3 devilEndPos = new Vector3(6, 1.5f, 0);

    private int randomValue()
    {
        float num = Random.Range(0f, 1f);
        if (num <= 0.5f) return 1;
        else return 2;
    }



    public IEnumerator nextMove()
    {

        if(side == 1 && priests_start.Count == 3 && devils_start.Count == 3)
        {
            int turn = randomValue();
            if (turn == 1)
            {
                priestStartOnBoat();
                devilStartOnBoat();
            } else
            {
                devilStartOnBoat();
                devilStartOnBoat();
            }
        }else if (side == 2 && priests_start.Count == 2 && devils_start.Count == 2)
        {
            
            priestEndOnBoat();
        } else if(side == 2 && priests_start.Count == 3 &&
                 devils_start.Count == 1)
        {
            devilEndOnBoat();
        } else if(side == 1 && priests_start.Count == 3 &&
                 devils_start.Count == 2)
        {

            devilStartOnBoat();
            devilStartOnBoat();
        } else if (side == 2 && priests_start.Count == 3 &&
                 devils_start.Count == 0)
        {
            devilEndOnBoat();
        } else if (side == 1 && priests_start.Count == 3 &&
                 devils_start.Count == 1)
        {
            priestStartOnBoat();
            priestStartOnBoat();
        } else if (side == 2 && priests_start.Count == 1 &&
                 devils_start.Count == 1)
        {
            priestEndOnBoat();
            devilEndOnBoat();
        } else if (side == 1 && priests_start.Count == 2 &&
                 devils_start.Count == 2)
        {
            priestStartOnBoat();
            priestStartOnBoat();
        } else if (side == 2 && priests_start.Count == 0 &&
                 devils_start.Count == 2)
        {
            devilEndOnBoat();
        } else if (side == 1 && priests_start.Count == 0 &&
                 devils_start.Count == 3)
        {
            devilStartOnBoat();
            devilStartOnBoat();
        } else if (side == 2 && priests_start.Count == 0 &&
                 devils_start.Count == 1)
        {
            int turn = randomValue();
            if(turn == 1)
            {
                devilEndOnBoat();
            } else
            {
                priestEndOnBoat();
            }
        } else if(side == 1 && priests_start.Count == 2 &&
                 devils_start.Count == 1)
        {
            priestStartOnBoat();
        } else if (side == 1 && priests_start.Count == 0 &&
                 devils_start.Count == 2)
        {
            devilStartOnBoat();
            devilStartOnBoat();
        } else if (side == 1 && priests_start.Count == 1 &&
                 devils_start.Count == 1)
        {
            priestStartOnBoat();
            devilStartOnBoat();
        }

        yield return new WaitForSeconds(1.0f);
        moveBoat();
        yield return new WaitForSeconds(1.0f);
        getOffTheBoat(1);
        getOffTheBoat(0);     
        
    }

    // 载入预置的游戏对象
    void Start()
    {
        GameSceneController.GetInstance().setGenGameObject(this);

        //Instantiate(Resources.Load("Prefabs/Directional Light"));
        Instantiate(Resources.Load("Prefabs/Shore"), shoreStartPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Shore"), shoreEndPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/River"), new Vector3(0,-0.5f,0), Quaternion.identity);
        boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), boatStartPos, Quaternion.identity) as GameObject;

        for (int i = 0; i < 3; ++i)
        {
            GameObject priest = Instantiate(Resources.Load("Prefabs/Priest")) as GameObject;
            priest.transform.position = getCharacterPosition(priestStartPos, i);
            priests_start.Push(priest);
            GameObject devil = Instantiate(Resources.Load("Prefabs/Devil")) as GameObject;
            devil.transform.position = getCharacterPosition(devilStartPos, i);
            devils_start.Push(devil);
        }
    }
    // 判断船上是否已满
    int boatCapacity()
    {
        int capacity = 0;
        for (int i = 0; i < 2; ++i)
            if (boat[i] == null) capacity++;
        return capacity;
    }
    // 实现上船动作，在这个过程中牧师或者魔鬼会以transform的形式到达船上
    void getOnTheBoat(GameObject obj)
    {
        if (boatCapacity() != 0)
        {
            obj.transform.parent = boat_obj.transform;
            Vector3 target = new Vector3();
            if (boat[0] == null)
            {
                boat[0] = obj;
                target = boat_obj.transform.position + new Vector3(-0.5f, 0.5f, 0);
            }
            else
            {
                boat[1] = obj;
                target = boat_obj.transform.position + new Vector3(0.5f, 0.5f, 0);
            }
            SSActionManager.GetInstance().ApplyCCMoveToYZAction(obj, target, speed);
        }
    }
    // 开船，直接调用SSActionManager中的方法即可
    public void moveBoat()
    {
        if (boatCapacity() != 2)
        {
            if (side == 1)
            {
                SSActionManager.GetInstance().ApplyCCMoveToAction(boat_obj, boatEndPos, speed);
                side = 2;
            }
            else if (side == 2)
            {
                SSActionManager.GetInstance().ApplyCCMoveToAction(boat_obj, boatStartPos, speed);
                side = 1;
            }
        }
    }
    // 下船，与上船的判断类似
    public void getOffTheBoat(int bside)
    {
        if (boat[bside] != null)
        {
            boat[bside].transform.parent = null;
            Vector3 target = new Vector3();
            if (side == 1)
            {
                if (boat[bside].tag == "Priest")
                {
                    priests_start.Push(boat[bside]);
                    target = getCharacterPosition(priestStartPos, priests_start.Count - 1);
                }
                else if (boat[bside].tag == "Devil")
                {
                    devils_start.Push(boat[bside]);
                    target = getCharacterPosition(devilStartPos, devils_start.Count - 1);
                }
            }
            else if (side == 2)
            {
                if (boat[bside].tag == "Priest")
                {
                    priests_end.Push(boat[bside]);
                    target = getCharacterPosition(priestEndPos, priests_end.Count - 1);
                }
                else if (boat[bside].tag == "Devil")
                {
                    devils_end.Push(boat[bside]);
                    target = getCharacterPosition(devilEndPos, devils_end.Count - 1);
                }
            }
            SSActionManager.GetInstance().ApplyCCMoveToYZAction(boat[bside], target, speed);
            boat[bside] = null;
        }
    }
    // 牧师或者魔鬼对上船方法的调用
    public void priestStartOnBoat()
    {
        if (priests_start.Count != 0 && boatCapacity() != 0 && side == 1) getOnTheBoat(priests_start.Pop());
    }
    public void priestEndOnBoat()
    {
        if (priests_end.Count != 0 && boatCapacity() != 0 && side == 2) getOnTheBoat(priests_end.Pop());
    }
    public void devilStartOnBoat()
    {
        if (devils_start.Count != 0 && boatCapacity() != 0 && side == 1) getOnTheBoat(devils_start.Pop());
    }
    public void devilEndOnBoat()
    {
        if (devils_end.Count != 0 && boatCapacity() != 0 && side == 2) getOnTheBoat(devils_end.Pop());
    }

    // 获取游戏对象的位置
    Vector3 getCharacterPosition(Vector3 pos, int index)
    {
        Debug.Log(pos.x);
        return new Vector3(pos.x + gap * index, pos.y, pos.z);
    }

    // 每一次上船或者下船之后，游戏都会进行相关变量的增减，同时判断当前游戏是否胜利或失败
    void Update()
    {
        GameSceneController scene = GameSceneController.GetInstance();
        int pOnb = 0, dOnb = 0;
        int priests_s = 0, devils_s = 0, priests_e = 0, devils_e = 0;

        if (priests_end.Count == 3 && devils_end.Count == 3)
        {
            scene.setMessage("Win!");
            return;
        }

        for (int i = 0; i < 2; ++i)
        {
            if (boat[i] != null && boat[i].tag == "Priest") pOnb++;
            else if (boat[i] != null && boat[i].tag == "Devil") dOnb++;
        }
        if (side == 1)
        {
            priests_s = priests_start.Count + pOnb;
            devils_s = devils_start.Count + dOnb;
            priests_e = priests_end.Count;
            devils_e = devils_end.Count;
        }
        else if (side == 2)
        {
            priests_s = priests_start.Count;
            devils_s = devils_start.Count;
            priests_e = priests_end.Count + pOnb;
            devils_e = devils_end.Count + dOnb;
        }
        if ((priests_s != 0 && priests_s < devils_s) || (priests_e != 0 && priests_e < devils_e)) scene.setMessage("Lose!");
    }
}