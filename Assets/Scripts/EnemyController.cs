﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public GameObject player;
	public float speed = 1;
	protected float timeLastAttacked = 0;
	public float distanceToAttack = 1.5f;
	public float coolDownMillis = 1500;
    public float knockbackMod = 100;
	public int atk;
    public BoardCreator creator;
    // Use this for initialization
    void Start () {
	}
	
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
			Attack();
		}

	}

	protected void Attack() {
        Vector2 knockback = player.transform.position - transform.position;
        knockback *= knockbackMod;
        player.GetComponent<PlayerController>().ReceiveDamage(atk, knockback);
		timeLastAttacked = Time.time * 1000;
	}

    public void Die() {
        Destroy(this.gameObject);
    }
}
