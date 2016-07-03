using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static string GameName = "Libera Player";

	#region Events declaration
	public delegate void GameEvent(string message);

	public static event GameEvent OnGameWin;
	public static event GameEvent OnGameStart;
	public static event GameEvent OnGameOver;
	#endregion

	public int BlockedCount; // conta i player bloccati.
	public int SafeCount; // conta i player salvati
	public int EnemiesCount; // conta i nemici nella scena;
	protected List<Player> Players = new List<Player>(); // lista di player in scena
	protected List<Player> SafePlayers = new List<Player>(); // lista di player salvati 
	protected List<Player> BlockedPlayers = new List<Player>(); // lista di player bloccati 
	public GameObject EnemyPrefab;
//	public ArrayList Enemies = new Array ();
	public GameObject[] BonusPrefabs;
	protected List<Enemy> Enemies = new List<Enemy>();
	public Transform[] SpawnPoints; // array di spawnPoints
	public Transform[] SpawnBonus; // array di spawnBonus
	float Cnt; //Contatore 
	public Transform[] EnemyPatrol; // array di punti che il nemico deve seguire mentre è in Patroling
	private float enemySpawnCounter; //Conta i nemici che vengono spawnati
	public float SpawnInterval; // Numero di secondi che passano prima che spawni un altro enemy
	public int LimitSpawnEnemy; //Numero massimo di nemici che vengono spawnati nella GameScene
	public int LimitSpawnBonus; // Numero massimo di bonus in scena
	public int bonusSpawnCounter; // Conta i bonus in scena.
	public int PlayersInScene;// conta i player in scena
	/// <summary>
	/// Checks if its time to spawn bonus.
	/// </summary>
	void CheckIfItsTimeToSpawnBonus(){
		if(CanSpawnBonus() == true){
			RandomSpawnBonus();
			bonusSpawnCounter ++;
		}
	}
	private bool CanSpawnBonus (){
		if (LimitSpawnBonus > bonusSpawnCounter) {
			return true;
		} else {
			return false;
		}
	} 

	void Awake(){
//		#region
//		// Subscribe -- void OnEnable()
//		GameController.OnGameStart += OnGameStartHandler;
//
//		// Unsubscribe -- void OnDisable()
//		GameController.OnGameStart -= OnGameStartHandler;
//		#endregion
	}

	void Start(){
		#region Event Test
		if(OnGameStart != null){
			OnGameStart("Si parte!");
		}
		#endregion
	}


	
	
	// Update is called once per frame
	 void Update () {
		//Contatore
		UpdateCnt ();
		if (checkCounter () == true) {
			if (CanSpawnEnemy () == true) {
				RandomSpawnEnemy ();
				enemySpawnCounter ++;
				Cnt = 0;
			}
		}
		CheckIfItsTimeToSpawnBonus();
	}
	void FixedUpdate(){

	}
	//Spawn generale
	void Spawn(GameObject objectToSpawn, Vector3 positionToSpawn){
		Instantiate (objectToSpawn, positionToSpawn, objectToSpawn.transform.rotation);

	}
	/// <summary>
	/// Spawn Random dei bon
	/// </summary>
	void RandomSpawnBonus (){
		int randomBouns = Random.Range(0, BonusPrefabs.Length);
		GameObject BonusToSpawn = BonusPrefabs [randomBouns];
		int randomIndex = Random.Range (0,SpawnBonus.Length -1);
		Vector3 spawnPosition = SpawnBonus [randomIndex].position;
		Spawn (BonusToSpawn,spawnPosition);
	}
	/// <summary>
	/// Spawn Random dei nemici
	/// </summary>
	void RandomSpawnEnemy (){	 
		GameObject enemyToSpawn = EnemyPrefab;
		int randomIndex = Random.Range (0,SpawnPoints.Length -1);
		Vector3 spawnPosition = SpawnPoints [randomIndex].position;
		Spawn (enemyToSpawn,spawnPosition);

	}

// aggiorna il contatore
	void UpdateCnt () { 
		Cnt = Cnt + Time.deltaTime;

	}




	// controlla genericamente se è il momento di fare qualcosa ( in questo caso lo spawn)
	public bool checkCounter(){
		if (Cnt >= SpawnInterval) {
			return true;
		}
		else {
			return false;
		}
	}


	/// <summary>
	/// Determines whether this instance can spawn enemy.
	/// </summary>
	/// <returns><c>true</c> if this instance can spawn enemy; otherwise, <c>false</c>.</returns>
	private bool CanSpawnEnemy (){
		if (LimitSpawnEnemy > enemySpawnCounter) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Richiamata quando e stato aggiunto un player.
	/// </summary>
	public void PlayerAdded(Player playerToAdd){
		Players.Add (playerToAdd);

		PlayersInScene ++;

	}

	public void SafePlayerAdded(Player safePlayerToAdd){
		SafePlayers.Add (safePlayerToAdd);
		
		SafeCount ++;
		
	}
	public void BlockedPlayersAdded(Player blockedPlayerToAdd){

		BlockedPlayers.Add (blockedPlayerToAdd);
		
		BlockedCount ++;

	}

	public void EnemyAdded(Enemy enemyToAdd){
		Enemies.Add (enemyToAdd);
		EnemiesCount ++;
	}

	/// <summary>
	/// Avvisa che un player ha cambiato stato.
	/// </summary>
	public void PlayerChangedState(){
	// SafeCount = 0;
		int BlockedCount = 0;
		foreach (Player currentPlayer in Players) {
			switch (currentPlayer.CurrentPlayerState) {
				case Player.PlayerStates.Safe:
//					SafeCount = SafeCount + 1;
					break;
				case Player.PlayerStates.Blocked:
					BlockedCount = BlockedCount + 1;
					break;
				case Player.PlayerStates.Free:
					return;
				default:
					Debug.LogWarningFormat("Stato {0} non atteso", currentPlayer.CurrentPlayerState.ToString());
				break;
			}
		}
		if (SafeCount == Players.Count) {
			if (OnGameWin != null){
				OnGameWin("Hai Vinto");
			}
		}else if(BlockedCount == Players.Count - SafeCount){

			if (OnGameOver != null) {
				OnGameOver("Hai Perso!");
			}
		}
	}

	public void DestroyEnemyBonus() {
		
	//Debug.Log ("Dovrei distruggere la metà dei nemici ma non lo faccio perchè i programmatori sono scemi");
		int culo = Enemies.Capacity/2;
		//List<Enemy> HalfList = Enemies.GetRange (0, Enemies.Count / 2);
		Enemies.RemoveRange(0, culo);

	}
}

