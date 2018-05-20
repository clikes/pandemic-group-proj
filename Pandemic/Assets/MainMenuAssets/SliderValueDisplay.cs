using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueDisplay : MonoBehaviour {

	Text textComponent;

	void Start() {
		textComponent = GetComponent<Text>();
	}

	public void UpdateText(float f) {
		textComponent.text = f.ToString();
	}
}