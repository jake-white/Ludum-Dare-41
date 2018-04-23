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
			dir = dir.normalized;
			GetComponent<Rigidbody2D>().AddForce(dir * speed);			
    }

	void OnCollisionEnter2D(Collision2D collider) {
		if(collider.gameObject.tag == "Player") {
			if(Time.time * 1000 > timeLastAttacked + coolDownMillis) {
				Attack();
			}
		}
	}
}
