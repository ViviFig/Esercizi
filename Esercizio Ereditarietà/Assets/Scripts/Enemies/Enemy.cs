using UnityEngine;
using System.Collections;

public class Enemy : Agent, IPatrol {



	public GameObject enemyPrefab;

	float patrolTimerCounter = 0; // contatore per cambio target player
	public float PatrolTimerLimit = 2.00f;
	public float PatrolTimerLimitMin = 2.00f;
	public float PatrolTimerLimitMax = 15.00f;
	Transform targetTransform;


	// richiama il metodo che setta i comportamenti di ogni stato solo quando necessario
	private AiState currentAiState = AiState.Follow; 
	public AiState ECurrentAiState {
		get{return currentAiState;}
		set{
			if (currentAiState != value){
				currentAiState = value;
				OnChangeState();
			}
			currentAiState = value;
		}
	}
	void Awake () {
	Player.OnBonusTaken += HandleOnBonusTaken;
	}

	void HandleOnBonusTaken ()
	{
		moveSpeed = moveSpeed - speedModifier * -2;
	}

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindObjectOfType<GameController>();
		ECurrentAiState = AiState.Patroling ;
		targetTransform = SelectRandomPatrolPointTarget();
		gameController.EnemyAdded(this);
	}

	void FixedUpdate(){
		updatePatrolCounterAndCheckLimit ();

		if(targetTransform != null){
		    MoveToTarget(targetTransform);
//			DestroyEnemy ();
		}
		if (moveSpeed <= 0) {
			DestroyEnemy();
		}
	}
	/// <summary>
	/// Updates the patrol counter and check limit.
	/// </summary>
	void updatePatrolCounterAndCheckLimit(){
		//Aggiorniamo il counter
		patrolTimerCounter = patrolTimerCounter + Time.deltaTime;
		//Confronto il contatore con il limit
		if (patrolTimerCounter >= PatrolTimerLimit) {
			// Seleziono un altro target in modo random
			if (currentAiState == AiState.Patroling) {
				targetTransform = SelectRandomPatrolPointTarget ();
				patrolTimerCounter = 0;
				PatrolTimerLimit = SetPatrolTimerLimit();
			} else {//}
				//SelectTarget ();
			}
		}
	}

	/// <summary>
	/// Setta il patrol time.
	/// </summary>
	float SetPatrolTimerLimit(){
		float newPatrolTimerLimit = Random.Range(PatrolTimerLimitMin, PatrolTimerLimitMax);
		return newPatrolTimerLimit;
	}

	void OnTriggerStay (Collider other){
		//Se il player e' nello stato Free, l'enemy lo segue 
		Player p = other.gameObject.GetComponent<Player> ();
		if (p != null && p.CurrentPlayerState == Player.PlayerStates.Free) {
			p.IsFollowed = true;
			targetTransform = other.gameObject.GetComponent<Transform>();
			ECurrentAiState = AiState.Follow;
		}
		//Se il player e' nello stato blocked, l'enemy non lo segue e torna a fare il patrol
		if (p != null && p.CurrentPlayerState == Player.PlayerStates.Blocked) {
			p.IsFollowed = false;
			ECurrentAiState = AiState.Patroling;
		}

	}

	void OnTriggerExit (Collider other) {
		
		Player p = other.gameObject.GetComponent<Player> ();
		if (p != null ) {
			targetTransform = null;
			ECurrentAiState = AiState.Patroling;
		}
	}

	/// <summary>
	/// Selects the random target.
	/// </summary>
	/// <returns>The random target.</returns>
	public Transform SelectRandomPatrolPointTarget(){
		//ECurrentAiState = AiState.Patroling ;
		int randomPoint = Random.Range (0, gameController.EnemyPatrol.Length -1);
		Transform selectedEnemyPatrol = gameController.EnemyPatrol [randomPoint];
			return selectedEnemyPatrol;
	}

	// segue il target
	public void MoveToTarget(Transform targetTransform){

		transform.LookAt (targetTransform.position);
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
	}

	// si muove verso il target scelto
	void SelectTarget () {
		
		//if (patrolTimerCounter >= PatrolTimerLimit && currentAiState == AiState.Patroling) {// se il contatore è maggiore del tempo d'attesa, sceglie e segue il patrolPoint

		targetTransform = SelectRandomPatrolPointTarget ();
		patrolTimerCounter = 0; // azzera il contatore
		if (targetTransform != null) { // si muove solo se il target è diverso da null
			MoveToTarget (targetTransform);
		} else {
			targetTransform = SelectRandomPatrolPointTarget();
				//SelectRandomTarget ();
			patrolTimerCounter = 0;
		}
	}


	// Stati del nemico
	public enum AiState {
		Patroling,
		Follow,
		Dead
	}

	// Setta i comportamenti in base allo stato del nemico
	public void OnChangeState (){

		switch (ECurrentAiState) {

		case AiState.Patroling:
			
			//Debug.Log("I'm patroling");
			UpdateSpeed (speedModifier);// quando è in patroling decrementa velocità ogni tot sec
			break;
		
		case AiState.Follow:
			//Debug.Log ("I'm following");
			break;
		
		case AiState.Dead:
			DestroyEnemy ();
			break;

		}
	}
	public void DestroyEnemy (){
	
			Destroy (this);
		
	}

	public void Patrol(){
		// Fai il patrol
	}

}
