using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour {

	public Text HPText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateHP(int hp, int maxhp) {
		HPText.text = hp + "/" + maxhp;
		GetComponent<RectTransform>().anchorMax = new Vector2((float) hp/ (float) maxhp, GetComponent<RectTransform>().anchorMax.y);
	}
	

}
