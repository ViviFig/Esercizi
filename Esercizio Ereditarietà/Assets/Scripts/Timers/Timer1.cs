using UnityEngine;
using System.Collections;

public class Timer1 : GenericTimer {

	protected override void Execute ()	{
		Debug.Log ("Timer 1 limit " + timeCounterLimit);
	}
}
