using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

	public int hp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(hp <= 0) {
			Die();
		}		
	}


	void Die() {
        this.transform.parent.gameObject.GetComponent<EnemyController>().Die();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Weapon") {
			hp -= collider.gameObject.GetComponent<WeaponScript>().atk;
		}
	}
}
