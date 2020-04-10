using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Playground/Conditions/Condition Collision")]
[RequireComponent(typeof(Collider2D))]
public class ConditionCollision : ConditionBase
{

	//This will create a dialog window asking for which dialog to add
	private void Reset()
	{
		Utils.Collider2DDialogWindow(this.gameObject, false);
	}
	
	// This function will be called when something touches the trigger collider
	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log($"Collision: {collision.collider.name}");
		if(collision.collider.CompareTag(filterTag)
			|| !filterByTag)
		{
			ExecuteAllActions(this.gameObject);
		}
	}
}
