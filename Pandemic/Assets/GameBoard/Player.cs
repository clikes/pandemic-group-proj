using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Photon.PunBehaviour {
    private List<PlayerCard> PlayerHand = new List<PlayerCard> ();
    public GameObject CurrentCity;
    public City City;
    public int PlayerID;
    public Role PlayerRole;

    public void MoveTo (GameObject City) {
        this.transform.position = City.transform.position; //TODO make it to right position
        CurrentCity = City;
        this.City = City.GetComponent<City>();
        //Medic here
        

    }
    // Use this for initialization

    public void AddToHand (PlayerCard Card) {
        if (PlayerHand.Count < 7) {
            PlayerHand.Add (Card);
        }
    }

    public void Showcard () { //test function
        Debug.Log (PlayerHand[0]);
        Debug.Log (PlayerHand[1]);
        Debug.Log (PlayerHand[2]);
        Debug.Log (PlayerHand[3]);

    }

    public void Discard (PlayerCard Card) {
        PlayerHand.Remove (Card);
    }

    public bool HasCard (PlayerCard Card) {
        foreach (var card in PlayerHand)
        {
            if (card == Card)//TODO test if work
            {
                return true;
            }
        }
        return false;
    }
    public void SetRole (Role role) {
        PlayerRole = role;
    }

    public List<PlayerCard> GetPlayerHand () {
        return PlayerHand;
    }
    void Start () {
        CurrentCity = GameObject.Find ("Atlanta");
        MoveTo (CurrentCity);
    }

    // Update is called once per frame
    void Update () {

    }
}