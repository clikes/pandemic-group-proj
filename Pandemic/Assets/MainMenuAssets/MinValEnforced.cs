using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinValEnforced : MonoBehaviour {
	Slider slider;

	void Start(){
		slider = GetComponent <Slider>();
	}

	public void UpdateLimits (bool c) {
		if (c){
			slider.minValue = 3;
			slider.maxValue = 5;
		}

		else {
			slider.minValue = 2;
			slider.maxValue = 4;
		}
	}
}


