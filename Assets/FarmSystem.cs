using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmSystem : MonoBehaviour {

	public Tilemap tilemap, seedmap;
	SeedSystem seedSystem;
	// Use this for initialization
	void Awake () {
		seedSystem = GameObject.Find("SeedSystem").GetComponent<SeedSystem>();
		seedSystem.tilemap = seedmap;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
