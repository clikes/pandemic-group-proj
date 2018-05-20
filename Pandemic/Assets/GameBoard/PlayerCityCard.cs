using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCityCard : MonoBehaviour {
	public GameObject correspongingCity;
	private GameObject front;
	private bool frontFaced;
	public int cardNumber;
	public bool ChooseCardMode = false;
    public bool ChooseCardToFlight = true;
	public sendRequest RequestMenu;
	public PlayerCard card;
    private GameManager gameManager;
	// Use this for initialization
	void Start () {
		front = transform.Find ("Front").gameObject;
		frontFaced = true; //set to false if you want cards to be initially back faced
                           //GameObject MenuObject = GameObject.FindGameObjectWithTag("RequestMenu");
                           //MenuObject.SetActive(false);
                           //RequestMenu = MenuObject.GetComponent<SendRequest> ();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnChooseCard (int eventcode) {
		if (ChooseCardMode) {
            RequestMenu.ShowChooseShareCardMenu("Trade " + card.ToString() + " ?", card);
            return;
        } else if (eventcode == Role.OperationsExpert.GetHashCode()){
            RequestMenu.ShowMenu("Discard " + card.ToString() + " and choose a city to move to" + " ?", Role.OperationsExpert.GetHashCode());
            gameManager.OperationsExpertAbilityCard = (PlayerCard)correspongingCity.GetComponent<City>().cityNumber;
            return;
        }
        RequestMenu.ShowChooseCityCardMenu("Flight to  " + card.ToString() + " ?", card);

    }

	public void SetCity (GameObject City) {
		correspongingCity = City;
	}

	public void Flip () {
		Debug.Log (correspongingCity);
        if (gameManager.currentPlayer.PlayerRole == Role.OperationsExpert || gameManager.currentPlayer.City.HasResearchStation())
        {
            OnChooseCard(Role.OperationsExpert.GetHashCode());
            return;
        }
		OnChooseCard (0);

		if (frontFaced) {
			//correspongingCity.SetActive (true);

			front.SetActive (false);
			frontFaced = false;
		} else {
			//correspongingCity.SetActive (false);
			front.SetActive (true);
			frontFaced = true;
		}

	}
}