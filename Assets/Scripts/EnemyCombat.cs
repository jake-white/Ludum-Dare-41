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
		Destroy(this.transform.parent.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log(collider.gameObject);
		if(collider.gameObject.tag == "Weapon") {
			hp -= collider.gameObject.GetComponent<WeaponScript>().atk;
		}
	}
}
