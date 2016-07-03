using UnityEngine;
using System.Collections;
	
public class Player : Agent {
	#region Events declaration
	public delegate void PlayerEvent();
	
	public static event PlayerEvent OnBonusTaken;
	#endregion

	Renderer rendBody; //corpo
	Player p;
	private Rigidbody rb;
	public Material LockedMat;
	public Material UnlockedMat;
	public Material Hat;
	public Material HatFollowed;
	Renderer hatRend; //cappello



	bool isFollowed;
	public bool IsFollowed {
		get{
			return isFollowed;
		}
		set{
			if(isFollowed != value){
				isFollowed = value;
				OnFollowStateChange();
			}

		}
	}

	// richiama il metodo che setta i comportamenti di ogni stato solo quando necessario
	private PlayerStates currentPlayerState = PlayerStates.Free; 
	public PlayerStates CurrentPlayerState {
		get{return currentPlayerState;}
		set{
			if (currentPlayerState != value){
				currentPlayerState = value;

				OnChangeState();

			}
			currentPlayerState = value;
		}
	}

	/// <summary>
	/// Raises the follow stae change event.
	/// </summary>
	void OnFollowStateChange(){
		if (isFollowed == true) {
			hatRend.material = HatFollowed;
		} else {
			hatRend.material = Hat;
		}
	}

	void Awake () {
		// Iscrizione all'evento GameController.OnGameStart
		GameController.OnGameStart += OnGameStartHandler;
		rb = GetComponent<Rigidbody> ();
		p = GetComponent <Player> ();
	}

	/// <summary>
	/// Funzione di risposta all'evento GameController.OnGameStart.
	/// </summary>
	/// <param name="eventMessage">Event message.</param>
	void OnGameStartHandler(string eventMessage){
		Debug.Log ("On Start " + this.name + " : " + eventMessage);
	}

	// Use this for initialization
	void Start () {
		rb.constraints = RigidbodyConstraints.FreezeRotationX;
		rb.constraints = RigidbodyConstraints.FreezeRotationY;
		rb.constraints = RigidbodyConstraints.FreezeRotationZ;
		rb.constraints = RigidbodyConstraints.FreezePositionY;
//		gameController.SafeCount = 0;
//		#region Event Test
//		if(OnBonusTake != null){
//			OnBonusTake("Go preso o bonus,boia can!");
//		}
//		#endregion
		gameController.PlayerAdded(this);

		rendBody = GetComponent<Renderer>(); 
		Renderer[] hats;
		hats = GetComponentsInChildren<Renderer>(); 
		hatRend = hats[1];
		moveSpeed = 4f;
		currentPlayerState = PlayerStates.Free;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Move ();

		}
			
		



	// Movimento con tastiera
	void Move () {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");
		rb.velocity = new Vector3 (moveH, 0, moveV)* moveSpeed;
	}

	// se un Player entra nel collider di un altro player lo stato dell'altro passa a Free
	// se un Player entra nel collider di un enemy lo stato del Player passa a Blocked
	void OnTriggerEnter (Collider other) {
		Player playerToFree = other.gameObject.GetComponent<Player> ();
		if (playerToFree != null) {
			playerToFree.CurrentPlayerState = PlayerStates.Free;

		} else if (other.tag == "BlockPoint") {
			CurrentPlayerState = PlayerStates.Blocked;
		}
		if (other.tag == "SafeArea") {
			CurrentPlayerState = PlayerStates.Safe;

		}
		if (other.tag == "Bonus") {
			if (OnBonusTaken != null) {
				Debug.Log ("Bonuspreso");
				OnBonusTaken ();
				Destroy (other.gameObject);
			}
		}
		if (other.tag == "BonusStraFigo") {
			gameController.DestroyEnemyBonus ();
			Debug.Log ("Ho preso il superbonus");
			Destroy (other.gameObject);
		}	
	}

	//Stati del Player
	public enum PlayerStates {
		Free,
		Blocked,
		Safe
	}

	/// <summary>
	/// Viene chiamata quando cambia il valore di CurrentAiState
	/// </summary>
	public void OnChangeState () {
		CanCount = false;
		switch (CurrentPlayerState) {
		case PlayerStates.Blocked:

			hatRend.material = Hat;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			rendBody.material = LockedMat;
	//		WaypointFollower wp = GetComponent<WaypointFollower>();// creo la variabile per distruggere il componente di tipo WP(script)
//			Component.Destroy(wp);
			gameObject.GetComponent<PlayerAudio>().PlaySound(PlayerAudio.Sounds.Blocked);
			gameController.PlayersInScene --;
			gameController.BlockedPlayersAdded (this);
			break;
		
		case PlayerStates.Free:
			rb.constraints = RigidbodyConstraints.FreezeRotationX;
			rb.constraints = RigidbodyConstraints.FreezeRotationY;
			rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			rb.constraints = RigidbodyConstraints.FreezePositionY;
			//Azzero il counter perchè mi hanno liberato e resetto speed uguale all'initialSpeed
			Counter = 0; //Azzera il counter se il player si libera
			moveSpeed = initialSpeed;
			rb.constraints = RigidbodyConstraints.None;
			rendBody.material = UnlockedMat;
		//	gameObject.AddComponent<WaypointFollower>();
			gameObject.GetComponent<PlayerAudio>().PlaySound(PlayerAudio.Sounds.Free);
			CanCount = true;
			gameController.BlockedCount --;
			gameController.PlayersInScene ++;
			break;
		
		case PlayerStates.Safe:
			Debug.Log ("Sono Salvo");
			this.gameObject.SetActive(false);
			gameController.PlayersInScene --;
			gameController.SafePlayerAdded (this);
			break;

		}

		gameController.PlayerChangedState ();
	}
	
}