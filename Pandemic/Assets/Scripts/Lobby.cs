using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {
	public settings settingsScript;
	public GameObject player;

	private int numberOfPlayers;
	private int numberOfEpidemicCards;
	private bool bioterrorist;
	private bool virulentStrain;
	private bool mutation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.numberOfPlayers = settingsScript.getNumberOfPlayers ();
		this.numberOfEpidemicCards = settingsScript.getNumberOfEpidemicCards ();
		this.bioterrorist = settingsScript.getBioterrorist ();
		this.virulentStrain = settingsScript.getVirulentStrain ();
		this.mutation = settingsScript.getMutation ();
	}
}
