using UnityEngine;
using System.Collections;

public class GenericTimer : MonoBehaviour {




	protected float timeCounter;
	public float timeCounterLimit;

	// Update is called once per frame
	void Update () {
		timeCounter = timeCounter + Time.deltaTime;
		if (timeCounter  >= timeCounterLimit) {
			timeCounter = 0;
			Execute();
		}
	}

	protected virtual void Execute(){ 

	}
}
