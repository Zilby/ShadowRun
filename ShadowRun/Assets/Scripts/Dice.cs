using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	private Rigidbody rbody;

	public Rigidbody Rbody
	{
		get { return rbody; }
	}

	private int? val;

	public int? Value
	{
		get { return val; }
	}

	private DiceValue[] values;

	private void Awake()
	{
		rbody = GetComponent<Rigidbody>();
		values = GetComponentsInChildren<DiceValue>();
	}

	public IEnumerator Toss()
	{
		val = null;
		float highestY = Mathf.NegativeInfinity;
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		while (rbody.velocity.magnitude > 0)
		{
			yield return new WaitForFixedUpdate();
		}
		foreach(DiceValue d in values)
		{
			if (val == null || d.transform.position.y > highestY)
			{
				val = d.value;
				highestY = d.transform.position.y;
			}
		}
	}
}
