using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyActionFactory : MonoBehaviour {
    private Dictionary<int, SSAction> used = new Dictionary<int, SSAction>();
    private List<SSAction> free = new List<SSAction>();
    private List<int> wait = new List<int>();

    public CCFlyAction Fly;

    // Use this for initialization  
    void Start()
    {
        Fly = CCFlyAction.GetCCFlyAction();
    }


    private void Update()
    {
        foreach (var tmp in used.Values)
        {
            if (tmp.destroy)
            {
                wait.Add(tmp.GetInstanceID());
            }
        }

        foreach (int tmp in wait)
        {
            FreeSSAction(used[tmp]);
        }
        wait.Clear();
    }


    /** 
     * GetSSAction这个函数是用来获取CCFlyAction这个动作的， 
     * 每次首次判断free那里还有没有未使用的CCFlyActon这个动作， 
     * 有就从free那里获取，没有就生成一个CCFlyAction 
     */

    public SSAction GetSSAction()
    {
        SSAction action = null;
        if (free.Count > 0)
        {
            action = free[0];
            free.Remove(free[0]);
            Debug.Log(free.Count);
        }
        else
        {
            action = ScriptableObject.Instantiate<CCFlyAction>(Fly);

        }

        used.Add(action.GetInstanceID(), action);
        return action;
    }

    public void FreeSSAction(SSAction action)
    {
        SSAction tmp = null;
        int key = action.GetInstanceID();
        if (used.ContainsKey(key))
        {
            tmp = used[key];
        }

        if (tmp != null)
        {
            tmp.reset();
            free.Add(tmp);
            used.Remove(key);
        }
    }

    public void clear()
    {
        foreach (var tmp in used.Values)
        {
            tmp.enable = false;
            tmp.destroy = true;

        }
    }
}
