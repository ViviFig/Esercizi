using UnityEngine;
using System.Collections;
/// <summary>
/// Conferisce la capacità al player di emettere suoni
/// </summary>
public class PlayerAudio : MonoBehaviour {

	AudioSource audioSource;
	public AudioClip Followed;
	public AudioClip Blocked;
	public AudioClip Free;
	public AudioClip GameOver;
	public AudioClip Win;
	public Sounds DefaultSound;

	void Awake () {
	GameController.OnGameOver += OnGameOverHandler;
	GameController.OnGameWin += OnGameWinHandler; 
	}

	void OnGameOverHandler (string message)
	{
		PlaySound (Sounds.GameOver);

	}
	void OnGameWinHandler (string message)
	{
		PlaySound (Sounds.Win);	
	}


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null) {
			audioSource = gameObject.AddComponent<AudioSource>(); 
		}

		PlaySound (DefaultSound); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(Sounds _soundToPlay){
		switch (_soundToPlay) {
		case Sounds.Blocked:
			audioSource.clip = Blocked;
			break;
		case Sounds.Followed:
			audioSource.clip = Followed;
			break;
		case Sounds.Free:
			audioSource.clip = Free;
			break;
		case Sounds.GameOver:
			audioSource.clip = GameOver;
			break;
			case Sounds.Win:
			audioSource.clip = Win;
			break;
		default:
			break;
		}

		audioSource.Play ();
	}

	public enum Sounds {
		Followed,
		Blocked,
		Free,
		GameOver,
		Win
	}
}
