using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolController : MonoBehaviour {

	public List<IPatrol> patrolAgents;

	public Enemy enemy1;
	public NPC npc1;

	// Use this for initialization
	void Start () {
		patrolAgents = new List<IPatrol>() { enemy1, npc1 };
	}
	
	// Update is called once per frame
	void Update () {

	}

	void StartPatrolForAll(){
		foreach (var item in patrolAgents) {
			item.Patrol();
		}
	}
}
