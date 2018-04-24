using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {
	
	public Text text;
	// Use this for initialization
	void Start () {
		UpdateSliderValue();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateSliderValue() {
		text.text = "" + GetComponent<Slider>().value;
	}

	public void Hide() {
		this.enabled = false;
		this.transform.localScale = new Vector3(0,0,0);
	}
}
