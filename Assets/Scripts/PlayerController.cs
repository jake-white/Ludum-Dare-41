using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed = 6.0F;
    public int hp = 10, maxhp = 10;
    private HPController HPC;
    public WeaponScript weapon;
    public int hungerLoss;
    public float hungerInterval;
    private float lastHunger;

    private SeedSystem seedSystem;
    private Vector3 moveDirection = Vector3.zero;
    Vector2 forceToBeAdded = Vector2.zero;
    private Rigidbody2D controller;
    private string lastHit = "Right";

    void Start()
    {
        // Store reference to attached component
        controller = GetComponent<Rigidbody2D>();
        HPC = GameObject.Find("HPC").GetComponent<HPController>();
        seedSystem = GameObject.Find("SeedSystem").GetComponent<SeedSystem>();
        hp = maxhp = seedSystem.maxhp();
        HPC.UpdateHP(hp, maxhp);
        lastHunger = Time.time * 1000;
        if(seedSystem.getAttackBuff()) {
            weapon.Buff();
        }
    }

    void Update()
    {
        if(Time.time * 1000 > lastHunger + hungerInterval) {
            lastHunger = Time.time * 1000;
            ReceiveDamage(hungerLoss, new Vector2(0, 0));
        }
        if(hp <= 0) {
            Die();
        }
        if (Input.GetButtonDown("Attack")) {
            if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Right")) {
                GetComponent<Animator>().Play("Stab_Right");
            }
            if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Left")) {
                GetComponent<Animator>().Play("Stab_Left");
            }
            if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Up")) {
                GetComponent<Animator>().Play("Stab_Up");
            }
            if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Down")) {
                GetComponent<Animator>().Play("Stab_Down");
            }
        }
        if(!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Stab_Right")
        && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Stab_Left")
        && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Stab_Up")
        && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Stab_Down")) {

            int directionsDown = 0;
            if (Input.GetButtonDown("Up")) {
                lastHit = "Up";
            }
            if (Input.GetButtonDown("Down")) {
                lastHit = "Down";
            }
            if (Input.GetButtonDown("Left")) {
                lastHit = "Left";
            }
            if (Input.GetButtonDown ("Right")) {
                lastHit = "Right";
            }

            if(Input.GetButton(lastHit)) {
                GetComponent<Animator>().Play(lastHit);
            }
            else if (Input.GetButton("Up") ) {
                GetComponent<Animator>().Play("Up");
            }
            else if (Input.GetButton("Down")) {
                GetComponent<Animator>().Play("Down");
            }
            else if (Input.GetButton("Left")) {
                GetComponent<Animator>().Play("Left");
            }
            else if (Input.GetButton("Right")) {
                GetComponent<Animator>().Play("Right");
            }


        }
        moveDirection = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* speed, 0.8f), Mathf.Lerp(0, Input.GetAxis("Vertical")* speed, 0.8f));
    }

    void FixedUpdate ()
    {
		controller.velocity = moveDirection;
        controller.AddForce(forceToBeAdded);
        forceToBeAdded = Vector2.zero;
    }

    public void collectSeed(int seed) {
        seedSystem.unlocks[seed] = true;
    }

    void Die() {
        SceneManager.LoadScene("GameOver");
    }

    public void ReceiveDamage(int atk, Vector2 knockback) {
        forceToBeAdded = knockback;
        HPC.UpdateHP(hp, maxhp);
        hp -= atk;
    }
}
