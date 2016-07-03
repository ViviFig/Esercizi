using UnityEngine;
using System.Collections;

public class RandomCase : MonoBehaviour {

	public int LimitForWin; //limite entro cui vinco

	// Use this for initialization
	void Start () {
		Debug.Log (GameController.GameName);
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyUp(KeyCode.Space)){
			calculate();
		
		}
		 }

	void calculate (){

	int result = Random.Range (1, 101);
	if (result > LimitForWin) { 
		Debug.LogFormat ("Ho vinto! {0} -> {1} " + result, LimitForWin); //format permette di mettere le graffe, e di riempirle con cio' che scrivo dopo
	} else {
		Debug.LogFormat ("Ho perso! {0} -> {1}" + result, LimitForWin);
	}
}
}

//Con lo switch case posso selezionare un caso quando esce un determinato range