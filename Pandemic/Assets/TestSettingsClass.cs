using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestSettingsClass : MonoBehaviour {
	public settings settingsScript;
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	public void Update () {
		text.text = "Number of Players = " + settingsScript.getNumberOfPlayers ().ToString () +
		                 "\nNumber of Epidemic Cards = " + settingsScript.getNumberOfEpidemicCards ().ToString () +
		                 "\nBioterrorist = " + settingsScript.getBioterrorist ().ToString () +
		                 "\nVirulent Strain = " + settingsScript.getVirulentStrain ().ToString () +
		                 "\nMutation = " + settingsScript.getMutation ().ToString ();

	}
}
