using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValueScript : MonoBehaviour {

	Text percentageText;

	// Use this for initialization
	void Start () {
		percentageText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void textUpdate (float value) {
		percentageText.text = Mathf.RoundToInt(value * 100) + "%";
	}
}
