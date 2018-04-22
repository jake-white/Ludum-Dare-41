using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update() {
		Vector3 dir = player.transform.position - transform.position;
		Debug.Log(player.transform.position + " " + transform.position);
		dir = dir.normalized;
		Debug.Log(dir);
		GetComponent<Rigidbody2D>().AddForce(dir * 2);
    }

	void Attack() {
		player.GetComponent<PlayerController>().ReceiveDamage(atk);
		timeLastAttacked = Time.time * 1000;
	}
}
