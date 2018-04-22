using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedSystem : MonoBehaviour {

	public bool[] unlocks = {true, false, false};
	public string[] names = {"Radishes", "Carrots", "Oats"};
	public int[] amounts = {20, 15, 5};
	public int[] food = {2,5,5};
	public string[] buffs = {"none", "none", "atk"};
	public string[] descriptions = {""};
	public Tile[] tiles;
	public Tilemap tilemap;
	public int[] inventory_amount;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool[] seedUnlocks() {
		return unlocks;
	
	}

	public string[] seedNames() {
		return names;
	}

	public int[] seedAmounts() {
		return amounts;
	}

	public int maxhp() {
		int sum = 0;
		for(int i = 0; i < inventory_amount.Length; ++i) {
			sum += inventory_amount[i] * food[i];
		}
		return sum;
	}

	public void Draw(int[] values) {
		inventory_amount = (int[]) values.Clone();
		for(int i = -3; i < 4; ++i) {
			for(int j = -3; j < 3; ++j) {
				bool keepGoing = true;
				for(int k = 0; k < values.Length && keepGoing; ++k) {
					if(values[k] > 0) {
						tilemap.SetTile(new Vector3Int(i,j,0), tiles[k]);
						values[k]--;
						keepGoing = false;
					}
				}
				if(keepGoing) {					
					tilemap.SetTile(new Vector3Int(i,j,0), null);
				}
			}
		}
		Debug.Log(inventory_amount[0]);
	}
}
