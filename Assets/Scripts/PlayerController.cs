using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed = 6.0F;
    public int hp = 10, maxhp = 10;
    private HPController HPC;
    public GameObject weapon;

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
    }

    void Update()
    {
        if(hp <= 0) {
            Die();
        }
        if (Input.GetButtonDown("Attack")) {
            weapon.GetComponent<Animator>().Play("Stab");
        }
        if(Input.GetButtonDown("Up")) {
            weapon.GetComponent<Animator>().Play("Up");
            GetComponent<Animator>().Play("Up");
        }
        if(Input.GetButtonDown("Down")) {
            weapon.GetComponent<Animator>().Play("Down");
            GetComponent<Animator>().Play("Down");
        }
        if(Input.GetButtonDown("Left")) {
            weapon.GetComponent<Animator>().Play("Left");
            GetComponent<Animator>().Play("Left");
        }
        if(Input.GetButtonDown("Right")) {
            weapon.GetComponent<Animator>().Play("Right");
            GetComponent<Animator>().Play("Right");
        }
            
        moveDirection = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* speed, 0.8f), Mathf.Lerp(0, Input.GetAxis("Vertical")* speed, 0.8f));
    }

    void FixedUpdate ()
    {
		controller.velocity = moveDirection;
    }

    void Die() {
        SceneManager.LoadScene("GameOver");
    }

    public void ReceiveDamage(int atk) {
        HPC.UpdateHP(hp, maxhp);
        hp -= atk;
    }
}
