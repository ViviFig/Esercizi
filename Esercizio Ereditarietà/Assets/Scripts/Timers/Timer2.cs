using UnityEngine;
using System.Collections;

public class Timer2 : GenericTimer {

	void faiquelcosadispeciale(){
		/// ....
	}

	protected override void Execute ()	{
		Debug.Log ("Timer 2 limit " + timeCounterLimit);
		faiquelcosadispeciale ();
	}
}	
