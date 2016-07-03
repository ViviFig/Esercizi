using UnityEngine;
using System.Collections;

public class NPC : Agent, IPatrol {

	public void Patrol(){
		// Fai qualcos'altro del patrol
		Helper1.IsOk (true);
	}

}
