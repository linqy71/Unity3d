using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 动作事件接口
public interface ISSActionCallback
{
    void OnActionCompleted(SSAction action);
}

public class SSActionManager : System.Object
{

    private static SSActionManager _instance;
    public static SSActionManager GetInstance()
    {
        if (_instance == null) _instance = new SSActionManager();
        return _instance;
    }
    // 实例对象在具体调用时只需要简单地修改接口中的参数即可实现移动
    public SSAction ApplyCCMoveToAction(GameObject obj, Vector3 target, float speed, ISSActionCallback completed)
    {
        CCMoveToAction ac = obj.AddComponent<CCMoveToAction>();
        ac.RunAction(target, speed, completed);
        return ac;
    }
    public SSAction ApplyCCMoveToAction(GameObject obj, Vector3 target, float speed)
    {
        return ApplyCCMoveToAction(obj, target, speed, null);
    }

    public SSAction ApplyCCMoveToYZAction(GameObject obj, Vector3 target, float speed, ISSActionCallback completed)
    {
        CCMoveToYZAction ac = obj.AddComponent<CCMoveToYZAction>();
        ac.RunAction(obj, target, speed, completed);
        return ac;
    }
    public SSAction ApplyCCMoveToYZAction(GameObject obj, Vector3 target, float speed)
    {
        return ApplyCCMoveToYZAction(obj, target, speed, null);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


public class CCMoveToYZAction : SSAction, ISSActionCallback
{
    public GameObject obj;
    public Vector3 target;
    public float speed;

    private ISSActionCallback monitor = null;

    public void RunAction(GameObject obj, Vector3 target, float speed, ISSActionCallback monitor)
    {
        this.obj = obj;
        this.target = target;
        this.speed = speed;
        this.monitor = monitor;
        GameSceneController.GetInstance().setMoving(true);

        if (target.y < obj.transform.position.y)
        {
            Vector3 targetZ = new Vector3(target.x, obj.transform.position.y, target.z);
            SSActionManager.GetInstance().ApplyCCMoveToAction(obj, targetZ, speed, this);
        }
        else
        {
            Vector3 targetY = new Vector3(target.x, target.y, obj.transform.position.z);
            SSActionManager.GetInstance().ApplyCCMoveToAction(obj, targetY, speed, this);
        }
    }

    public void OnActionCompleted(SSAction action)
    {
        SSActionManager.GetInstance().ApplyCCMoveToAction(obj, target, speed, null);
    }

    void Update()
    {
        if (obj.transform.position == target)
        {
            GameSceneController.GetInstance().setMoving(false);
            if (monitor != null) monitor.OnActionCompleted(this);
            Destroy(this);
        }
    }
}