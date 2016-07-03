using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

	Text[] texts ;
	public Text TotalPlayers;
	public Text SafePlayers;
	public Text BlockedPlayers;
	public Text Enemies;
	// Use this for initialization
	public GameController gc;
	int genericCounter;

	void Awake(){

		texts = GetComponentsInChildren<Text>(); 
		TotalPlayers = texts [1];
		SafePlayers = texts [2];
		BlockedPlayers = texts [3];
		Enemies = texts [4];
		GameController.OnGameOver += HandleOnGameOver;
		GameController.OnGameStart += HandleOnStart;
		GameController.OnGameWin += HandleOnGameWin;
	}

	void Update () {
		UpdateCanvas ();
	}

	void HandleOnStart (string message)
	{
		gameObject.GetComponentInChildren<Text> ().enabled = false; 
	}
	void HandleOnGameOver (string message)
	{
		gameObject.GetComponentInChildren<Text>().text = "GameOver";
		gameObject.GetComponentInChildren<Text> ().enabled = true;
	}

	void HandleOnGameWin (string message)
	{
		gameObject.GetComponentInChildren<Text>().text = "YouWin";
		gameObject.GetComponentInChildren<Text> ().enabled = true;
	}

	void UpdateCanvas (){
		SafePlayers.text = "Safe Players: " + gc.SafeCount;
		TotalPlayers.text = "Total Players: " + gc.PlayersInScene;
		BlockedPlayers.text = "Blocked Players: " + gc.BlockedCount;
		Enemies.text = "Enemies : " + gc.EnemiesCount;
	}
}
