using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Larvae : EnemyController {

	public bool hatched = false;
	float timeGenerated, timeLastMultiplied;
	public float hatchTime, multiplyCooldown;
	public GameObject larvaePrefab;

	// Use this for initialization
	void Start () {
		timeGenerated = Time.time * 1000;		
	}
	
	// Update is called once per frame
	void Update () {
		if(!hatched && (Time.time * 1000) > timeGenerated + hatchTime) {
			hatched = true;
		}
		if(hatched && Time.time * 1000 > timeLastMultiplied + multiplyCooldown) {
			timeLastMultiplied = Time.time*1000;
			Instantiate(larvaePrefab);
		}
		Vector2 flatPlayerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		Vector2 flatEnemyPos = new Vector2(transform.position.x, transform.position.y);
		float distanceToPlayer = Vector2.Distance(flatEnemyPos, flatPlayerPos);
		if(hatched) {
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			if(distanceToPlayer < 10) {
				agent.SetDestination(new Vector3(player.transform.position.x, player.transform.position.y, 0));
			}
			else { 
				agent.SetDestination(transform.position);
			}
		}

		if(distanceToPlayer <= distanceToAttack && Time.time * 1000 > timeLastAttacked + coolDownMillis) {
			Attack();
		}
		
	}
}
