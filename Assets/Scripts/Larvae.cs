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
		timeGenerated = timeLastMultiplied = Time.time * 1000;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 flatPlayerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 flatEnemyPos = new Vector2(transform.position.x, transform.position.y);
        float distanceToPlayer = Vector2.Distance(flatEnemyPos, flatPlayerPos);
        if (!hatched && (Time.time * 1000) > timeGenerated + hatchTime & distanceToPlayer < 10) {
            GetComponent<Animator>().Play("Wiggle");
			hatched = true;
            timeLastMultiplied = Time.time * 1000;
		}
        bool isTimeToMultiply = creator.LarvaeCanMultiply() && hatched && (Time.time * 1000) > (timeLastMultiplied + multiplyCooldown);

        if (isTimeToMultiply) {
            creator.LarvaeMultiply();
			timeLastMultiplied = Time.time*1000;
			Instantiate(larvaePrefab, transform.position, Quaternion.identity);
		}
        if (hatched) {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (distanceToPlayer < 10) {
                agent.SetDestination(new Vector3(player.transform.position.x, player.transform.position.y, 0));
            }
            else {
                agent.SetDestination(transform.position);
            }

            if (distanceToPlayer <= distanceToAttack && Time.time * 1000 > timeLastAttacked + coolDownMillis) {
                Attack();
            }
        }

    }

    public new void Die() {
        creator.LarvaeDie();
        Destroy(this.gameObject);
    }
}
