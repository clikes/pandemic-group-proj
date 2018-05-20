using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour {
    public GameObject Hand;
    public Player player1;
    public Player player2;
    public Player player3;
    public sendRequest RequestMenu;
    public Player CurrentPlayer;

    //private PlayerCard[] CurrentHand = new PlayerCard[8];
    private List<PlayerCard> CurrentHand = new List<PlayerCard> ();
    //ivate PlayerCard[] PlayerHand = new PlayerCard[8];
    //private PlayerCityCard[] CurrentHand = new PlayerCityCard[8];
    //private PlayerCityCard card = new PlayerCityCard();
    private GameObject[] CardHolder = new GameObject[8];
    // Use this for initialization
    void Start () {
        // for (int i = 0; i < 8; i++) {
        //     CurrentHand[i] = PlayerCard.Empty;
        // }
        RectTransform rectT = Hand.GetComponent<RectTransform> ();
        for (int i = 0; i < 8; i++) {
            CardHolder[i] = rectT.GetChild (i).gameObject;
            CardHolder[i].SetActive (false);
        }
        GameObject MenuObject = GameObject.FindGameObjectWithTag("RequestMenu");
        MenuObject.SetActive(false);
        RequestMenu = MenuObject.GetComponent<sendRequest> ();
        for (int i = 0; i < 8; i++) {
            PlayerCityCard card = CardHolder[i].GetComponent<PlayerCityCard> ();
            card.RequestMenu = RequestMenu;
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public void GetCurrentHand (int PlayerNumber) {
        //Debug.Log(PlayerNumber);
        switch (PlayerNumber) {
            case 1:
                CurrentHand = player1.GetPlayerHand ();
                CurrentPlayer = player1;
                break;
            case 2:
                CurrentHand = player2.GetPlayerHand ();
                CurrentPlayer = player2;
                break;
            case 3:
                CurrentHand = player3.GetPlayerHand ();
                CurrentPlayer = player3;
                break;
            default:
                return;
        }
        // CurrentHand[0] = PlayerCard.Tokyo;
        // CurrentHand[1] = PlayerCard.Bogota;
        // CurrentHand[2] = PlayerCard.Chicago;
        ShowHand ();
    }

    public void ShowHand () {
        int i = 0;

        // script = GetComponent ("ScriptName");
        // script.DoSomething ();
        for (i = 0; i < CurrentHand.Count; i++) {
            CardHolder[i].SetActive (true);
            PlayerCityCard card = CardHolder[i].GetComponent<PlayerCityCard> ();
            //Debug.Log (CurrentHand[i]);
            card.correspongingCity = GameObject.Find (CurrentHand[i].ToString ());
            card.card = CurrentHand[i];

            RectTransform rect = CardHolder[i].GetComponent<RectTransform> ();

            rect.GetChild (1).GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/PlayerCityCard/Pc-" + CurrentHand[i].ToString ());
        }

        while (i < 8) {
            CardHolder[i++].SetActive (false);
        }

    }

    public void ChooseCardModeOn () {
        for (int i = 0; i < 8; i++) {
            PlayerCityCard card = CardHolder[i].GetComponent<PlayerCityCard> ();
            card.ChooseCardMode = true;
        }
    }

    public void ChooseCardModeOff () {
        for (int i = 0; i < 8; i++) {
            PlayerCityCard card = CardHolder[i].GetComponent<PlayerCityCard> ();
            card.ChooseCardMode = false;
        }
    }

    bool collapseFlag = true;

    public void collapse () {
        if (collapseFlag) {
            for (int i = 0; i < 8; i++) {
                CardHolder[i].SetActive (false);
            }
            collapseFlag = !collapseFlag;
        } else {
            ShowHand ();
            collapseFlag = !collapseFlag;
        }

    }

    public void expand () {

    }
}