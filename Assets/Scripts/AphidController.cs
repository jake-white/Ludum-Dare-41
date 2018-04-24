using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class AphidController : EnemyController {
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 flatPlayerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		Vector2 flatEnemyPos = new Vector2(transform.position.x, transform.position.y);
		float distanceToPlayer = Vector2.Distance(flatEnemyPos, flatPlayerPos);
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		if(distanceToPlayer < 10) {
			agent.SetDestination(new Vector3(player.transform.position.x, player.transform.position.y, 0));
		}
		else { 
			agent.SetDestination(transform.position);
		}

		if(distanceToPlayer <= distanceToAttack && Time.time * 1000 > timeLastAttacked + coolDownMillis) {
      GetComponent<Animator>().Play("Attack");
			Attack();
		}

	}
}
