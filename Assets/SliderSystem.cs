using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderSystem : MonoBehaviour {

	public Slider[] sliders;
	public Button plantButton;
	public SeedSystem seedSystem;
	public Text total;
	public int plots;
	bool valid;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < sliders.Length; ++i) {
			if(!seedSystem.seedUnlocks()[i]) {
				sliders[i].GetComponent<SliderValue>().Hide();
			}
			sliders[i].maxValue = seedSystem.seedAmounts()[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateTotal() {
		int[] sliderValues = new int[sliders.Length];
		int sum = 0;
		for(int i = 0; i < sliders.Length; ++i) {
			sum += (int) sliders[i].value;
			sliderValues[i] = (int) sliders[i].value;
			
		}
		total.text = "" + sum;
		if(sum > plots) {
			plantButton.enabled = false;
			total.color = Color.red;
		}
		else {
			plantButton.enabled = true;
			total.color = Color.black;
		}
		seedSystem.Draw(sliderValues);
	}

	public void AdvanceGame() {
		SceneManager.LoadScene("Floor_1");
	}
}
