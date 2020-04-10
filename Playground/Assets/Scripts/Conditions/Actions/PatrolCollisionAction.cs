using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCollisionAction : Action
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public override bool ExecuteAction(GameObject dataObject)
	{

        Debug.Log($"Collided: {dataObject.name}");
        var patrol = dataObject.GetComponent<Patrol>();
        patrol.SwitchWaypoint();
        return true;
	}
}
