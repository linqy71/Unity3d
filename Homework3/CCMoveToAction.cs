using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction {

    public Vector3 target;
    public float speed;

    private ISSActionCallback monitor = null;

    public void RunAction(Vector3 target, float speed, ISSActionCallback monitor)
    {
        this.target = target;
        this.speed = speed;
        this.monitor = monitor;
        GameSceneController.GetInstance().setMoving(true);
    }

	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (transform.position == target)
        {
            GameSceneController.GetInstance().setMoving(false);
            if (monitor != null) monitor.OnActionCompleted(this);
            Destroy(this);
        }
    }
}
