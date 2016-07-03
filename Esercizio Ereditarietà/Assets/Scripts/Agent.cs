using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
	

	public float moveSpeed;
	public GameController gameController;
	public float speedModifier = 0; // float che aggiunto o sottratto alla velocità ne modifica il valore
	public float Counter;
	public float CounterLimit = 3; //Counter per limitare la modifica della velocità
	protected bool CanCount = true;
	/// <summary>
	/// Contiene il valore iniziale della movespeed.
	/// </summary>
	protected float initialSpeed;

	// Use this for initialization
	void Start () {
		initialSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	 void Update () {
	    if (CanCount == true){
		Counter = Counter + Time.deltaTime;
		if (Counter >= CounterLimit) {
			UpdateSpeed (speedModifier); // modifica la Speed usando lo SpeedModifier
				Counter = 0; 
			}
		}
	}

	/// <summary>
	/// Aggiorna il valore della speed attuale (moveSpeed) sommando il valore del parametro di entrate
	/// </summary>
	/// <param name="peedToAddOrRemove">Valore da aggiungere (se è negativo si sottrae).</param>
	public void UpdateSpeed (float SpeedToAddOrRemove) {
		moveSpeed = moveSpeed + SpeedToAddOrRemove;
		// per non fare andare la velocità in negativo 
		if (moveSpeed<=0) { 
			moveSpeed = 0;
		}
	}

	}

	

