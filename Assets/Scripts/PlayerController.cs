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

    private SeedSystem seedSystem;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D controller;

    void Start()
    {
        // Store reference to attached component
        controller = GetComponent<Rigidbody2D>();
        HPC = GameObject.Find("HPC").GetComponent<HPController>();
        seedSystem = GameObject.Find("SeedSystem").GetComponent<SeedSystem>();
        hp = maxhp = seedSystem.maxhp();
        HPC.UpdateHP(hp, maxhp);
        if(seedSystem.getAttackBuff()) {
            weapon.Buff();
        }
    }

    void Update()
    {
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
        if(Input.GetButtonDown("Up")) {
            GetComponent<Animator>().Play("Up");
        }
        if(Input.GetButtonDown("Down")) {
            GetComponent<Animator>().Play("Down");
        }
        if(Input.GetButtonDown("Left")) {
            GetComponent<Animator>().Play("Left");
        }
        if(Input.GetButtonDown("Right")) {
            GetComponent<Animator>().Play("Right");
        }   
        moveDirection = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* speed, 0.8f), Mathf.Lerp(0, Input.GetAxis("Vertical")* speed, 0.8f));
    }

    void FixedUpdate ()
    {
		controller.velocity = moveDirection;
    }

    public void collectSeed(int seed) {
        seedSystem.unlocks[seed] = true;
    }

    void Die() {
        SceneManager.LoadScene("GameOver");
    }

    public void ReceiveDamage(int atk) {
        HPC.UpdateHP(hp, maxhp);
        hp -= atk;
    }
}
