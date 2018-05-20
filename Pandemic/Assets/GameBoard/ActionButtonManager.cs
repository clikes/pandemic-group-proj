using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonManager : Photon.PunBehaviour
{
    public GameObject CurrentCity;
    public Player player1;
    public Player player2;
    public Player player3;
    public bool disable = true;
    public Player currentPlayer;
    // Use this for initialization
    void Start () {
        CurrentCity = null;
        
    }
	public void SetCurrentCity(GameObject City){
        CurrentCity = City;
    }
    public void Drive()
    {
        currentPlayer.MoveTo(CurrentCity);
        gameObject.SetActive(false);

    }

    public void TakeDirectFlight()
    {
        currentPlayer.MoveTo(CurrentCity);
        
        currentPlayer.Discard((PlayerCard)System.Enum.Parse(typeof(PlayerCard), CurrentCity.name));

    }
	// Update is called once per frame
	void Update () {
		
	}
}
