using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectTreasure : MonoBehaviour {

	public string NextScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetScene(string scene) {
		this.NextScene = scene;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Player") {
			SceneManager.LoadScene(NextScene);
		}
	}
}
