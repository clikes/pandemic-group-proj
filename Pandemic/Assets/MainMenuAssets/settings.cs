using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class settings : MonoBehaviour {

	private  int numberOfPlayers;
	private  int numberOfEpidemicCards;
	private  bool bioterrorist;
	private  bool virulentStrain;
	private  bool mutation;

	// Use this for initialization
	void Start () {
		numberOfPlayers = 2;
		numberOfEpidemicCards = 2;
		bioterrorist = false;
		virulentStrain = false;
		mutation = false;
	}

	public void setNumberOfPlayers (float number){
		numberOfPlayers = (int) number;
	}

	public void setNumberOfEpidemicCards(float number){
		numberOfEpidemicCards = (int) number;
	}

	public void setBioterrorist(bool check){
		bioterrorist = check;
	}

	public void setVirulentStrain(bool check){
		virulentStrain = check;
	}

	public void setMutation(bool check){
		mutation = check;
	}


	public int getNumberOfPlayers()
	{
		int numberOfPlayers = this.numberOfPlayers;
		return numberOfPlayers;
	}

	public int getNumberOfEpidemicCards()
	{
		int numberOfEpidemicCards = this.numberOfEpidemicCards;
		return numberOfEpidemicCards;
	}

	public bool getBioterrorist()
	{
		bool bioterrorist = this.bioterrorist;
		return bioterrorist;
	}

	public bool getVirulentStrain()
	{
		bool virulentStrain = this.virulentStrain;
		return virulentStrain;
	}

	public bool getMutation()
	{
		bool mutation = this.mutation;
		return mutation;
	}

}