using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Photon.PunBehaviour {

    public Player player1;
    public Player player2;
    public Player player3;
    //public Player player4;
    private Player[] players = new Player[5];
    public DeckManager deck;
    public HandManager hand;
    public GameObject PlayerButtons;
    public ActionButtonManager ActionButton;
    public CityManager cityManager;
    public sendRequest RequestMenu;

    public Player currentPlayer;

    private bool isYourTurn = false;
    private int MaxAction = 4;
    private int ActionTook = 0;

    public GameObject blueDisease;
    public GameObject redDisease;
    public GameObject yellowDisease;
    public GameObject blackDisease;

    public bool redCureFlag = false;
    public bool blueCureFlag = false;
    public bool yellowCureFlag = false;
    public bool blackCureFlag = false;
    public bool purpleCureFlag = false;

    public bool OnOperationsExpertAbility = false;
    public PlayerCard OperationsExpertAbilityCard;
    // Use this for initialization
    private void OnEventHandler (byte eveCode, object content, int senderId) {
        byte[] selected = content as byte[];
        Debug.LogError (eveCode + " " + senderId + " " + PhotonNetwork.player.ID);
        if (eveCode != (byte) 0) {
            return;
        }
        if (selected == null) {
            Debug.LogError ("111");
        } else if (selected[0] == (byte) 1) {
            Debug.LogError ("000");
        }
    }

    private void OnEventHandler1 (byte eveCode, object content, int senderId) {
        byte[] selected = content as byte[];
        Debug.LogError (eveCode + " " + senderId + " " + PhotonNetwork.player.ID);
        if (eveCode != (byte) 3) {
            return;
        }
        if (selected == null) {
            Debug.LogError ("11111");
        } else if (selected[1] == (byte) 1) {
            Debug.LogError ("000");
        } else {
            Debug.LogError ("322");
        }
        Debug.LogError ("333");
    }

    private void testCityhandler (byte eveCode, object content, int senderId) {
        if (eveCode != (byte) 100) {
            return;
        }
        PlayerCityCard script = (PlayerCityCard) GameObject.Find ("PlayerCityCard0").GetComponent ("PlayerCityCard");
        script.Flip ();
    }
    public void test () {
        PhotonNetwork.RaiseEvent ((byte) 100, null, true, null);
    }

    private void DistributeCardEvent (byte eveCode, object content, int senderId) {
        if (eveCode != (byte) 101) {
            return;
        }
        //Debug.LogError ("101 " + PhotonNetwork.room.PlayerCount + " " + players[0].name);
        //players[0].addToHand(PlayerCard.Beijing);
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++) {
            for (int j = 0; j < 5; j++) {
                PlayerCard temp = deck.DrawPlayerCard ();
                //Debug.Log(temp);
                players[i].AddToHand (temp);
            }
        }
        hand.GetCurrentHand (PhotonNetwork.player.ID);
        //Debug.LogError ("101 " + PhotonNetwork.room.PlayerCount);
    }

    private void InitializeEvent (byte eveCode, object content, int senderId) {
        if (eveCode != 99) {
            return;
        }
        int seed = (int) content;
        deck.ShuffleCardInitcialize (seed);

        //Assign Role
        int RoleCounts = 23;
        int role;
        System.Random rng = new System.Random (seed);
        ArrayList Roles = new ArrayList ();
        for (int i = 0; i < RoleCounts; i++) {
            Roles.Add (i);
        }
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++) {
            RoleCounts--;
            role = rng.Next (RoleCounts);
            players[i].SetRole ((Role) Roles[role]);

            Roles.RemoveAt (role);
        }

        //distribute Cards
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++) {
            for (int j = 0; j < 5; j++) {
                PlayerCard temp = deck.DrawPlayerCard ();
                //Debug.Log(temp);
                players[i].AddToHand (temp);
            }
        }
        hand.GetCurrentHand (PhotonNetwork.player.ID);

        //placeRestation
        City Atlanta = GameObject.Find("Atlanta").GetComponent<City>();
        Atlanta.SetResearchStation(PlaceResearchStation(GameObject.Find("Atlanta"), ResearchStation));

        //Infect cities
        int count = 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                InfectionCard temp = deck.DrawInfectionCard();
                deck.PutInDiscardPile(temp);
                //Initialize disease cube counters for city
                initializeDiseaseCubesForCity(temp);

                for (int z = 0; z < count; z++)
                {
                    InfectCityByCard(temp);
                }
            }
            count -= 1;
        }
    }
    private void ShuffleCardEvent (byte eveCode, object content, int senderId) {
        if (eveCode != (byte) 102) {
            return;
        }
        int seed = (int) content;
        deck.ShuffleCardInitcialize (seed);
    }

    // shareknowledge event part may be attach to player script is better
    //bool isSendShareKnowledge = false;
    private bool OnShareKnowledge = false;
    public void OnClickShareKnowledge(){

        ActionButton.gameObject.SetActive(false);
        RequestMenu.ShowMessage("Select card from hand to ShareKnowledge.");
        hand.ChooseCardModeOn();
        OnShareKnowledge = true;
    }

    public void SelectCardCallBack(PlayerCard TradeCard)
    {
        if (OnShareKnowledge)
        {
            ShareKnowledge(1, TradeCard);
            OnShareKnowledge = false;
            hand.ChooseCardModeOff();// choose card to share
            return;
        }
        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)110, TradeCard.ToString(), true, opt);
        PhotonNetwork.RaiseEvent((byte)77, TradeCard.GetHashCode(), true, opt);
        ActionCallBack();

    }


    
    public void ShareKnowledge (int PlayerID, PlayerCard Card) {
        RaiseEventOptions opt = new RaiseEventOptions {
            Receivers = ReceiverGroup.All
        };
        ///send playerid senderid card hashcode and a placeholder for request
        ///
        byte[] content = { (byte)PlayerID, (byte)PhotonNetwork.player.ID, (byte)Card.GetHashCode (),(byte) 1};
        for (int i = 0; i < 4; i++)
        {
            //shareKnowledgeRequestContent[i] = content[i];
        }
        Debug.Log(content.Length);
        string debug = "";
        for (int i = 0; i < 3; i++)
        {
            debug += content[i].ToString() + " ";
        }
        Debug.Log("in ShareKnoledge " + debug);


        PhotonNetwork.RaiseEvent ((byte)103, content, true, opt);

    }

    private int[] shareKnowledgeRequestContent;

    private void ShareKnowledgeEvent (byte evCode, object content, int senderId) {
        
        if (evCode != (byte) 103) {
            return;
        }
        //int[] cont = content as int[];
        byte[] cont = content as byte[];
        shareKnowledgeRequestContent = new int[4];
        for (int i = 0; i < 4; i++)
        {
            shareKnowledgeRequestContent[i] = cont[i];
        }
        //string debug = "";
        Debug.LogError("Player" + PhotonNetwork.player.ID + " store content in local");
        string debug = "";
        for (int i = 0; i < 4; i++)
        {
            debug += shareKnowledgeRequestContent[i].ToString() + " ";
        }
        Debug.Log("content is " + debug);
        

        //Debug.Log("in ShareKnoledge " + debug);

        if (cont[0] != PhotonNetwork.player.ID) {
            return;
        }
        PlayerCard tradecard = (PlayerCard)cont[2];
        //int request = -1; //TODO get request
        // while (RequestMenu.GetRequest() == -1) ;
        string mesg = "You want to trade " + tradecard.ToString() + " with Player" + senderId + "?";
        //RequestMenu.ShowMenu(mesg, cont[0], cont[1], cont[2], cont[3]);
        RequestMenu.ShowMenu(mesg,1);//enent type 1 send request back
        
    }

    public void RequestCallback(bool request)
    {
        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        if (shareKnowledgeRequestContent == null)
        {
            return;
        }
        PhotonNetwork.RaiseEvent((byte)50, request, true, opt);
        //shareKnowledgeRequestContent = null;
    }

    private void NewShareKnowledgeRequestEvent(byte evCode, object content, int senderId)
    {
        if (evCode != (byte)50)
        {
            return;
        }

        bool request = (bool)content;
        if (!request)
        {
            if (shareKnowledgeRequestContent[1] == PhotonNetwork.player.ID)
            {
                //GUI player cancel
                RequestMenu.ShowMessage("Player" + shareKnowledgeRequestContent[0] + " rejects!");
            }
            return;
        }
        PlayerCard ShareCard = (PlayerCard)shareKnowledgeRequestContent[2];
        if (players[shareKnowledgeRequestContent[0] - 1].HasCard(ShareCard))
        {

            players[shareKnowledgeRequestContent[0] - 1].Discard(ShareCard);
            players[shareKnowledgeRequestContent[1] - 1].AddToHand(ShareCard);

        }
        else
        {
            players[shareKnowledgeRequestContent[1] - 1].Discard(ShareCard);
            players[shareKnowledgeRequestContent[0] - 1].AddToHand(ShareCard);
        }

        if (PhotonNetwork.player.ID == shareKnowledgeRequestContent[1])
        {
            hand.GetCurrentHand(PhotonNetwork.player.ID);
            ActionCallBack();
        }
        if (PhotonNetwork.player.ID == shareKnowledgeRequestContent[0])
        {
            hand.GetCurrentHand(PhotonNetwork.player.ID);
        }
        shareKnowledgeRequestContent = null;
        //bool request = (bool) content; //TODO shareknowledge or not

    }

    public void RequestCallback(int[] cont)
    {
        string debug = "";
        for (int i = 0; i < 4; i++)
        {
            debug += cont[i].ToString() + " ";
        }
        Debug.Log("in RequestCallback " + debug);

        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        if (shareKnowledgeRequestContent == null)
        {
            return;
        }
        PhotonNetwork.RaiseEvent((byte)104, cont, true, opt);
        shareKnowledgeRequestContent = null;
    }

    

    private void ShareKnowledgeRequestEvent (byte evCode, object content, int senderId) {
        if (evCode != (byte) 104) {
            return;
        }
        
        int[] cont = content as int[];

        string debug = "";
        for (int i = 0; i < 4; i++)
        {
            debug += cont[i].ToString() + " ";
        }
        Debug.Log("in ShareKnowledgeRequestEvent " + debug);

        for (int i = 0; i<4; i++)
        {
            Debug.LogError(cont[i]); 
        }
        

        if (cont[3] == 0) {
            if (cont[1] == PhotonNetwork.player.ID) {
                //GUI player cancel
                RequestMenu.ShowMessage("Player"+cont[0]+" rejects!");
            }
            return;
        }
        PlayerCard ShareCard = (PlayerCard) cont[2];
        if (players[cont[0] - 1].HasCard (ShareCard)) {

            players[cont[0] - 1].Discard (ShareCard);
            players[cont[1] - 1].AddToHand (ShareCard);

        } else {
            players[cont[1] - 1].Discard (ShareCard);
            players[cont[0] - 1].AddToHand (ShareCard);
        }

        if (PhotonNetwork.player.ID == cont[1])
        {
            hand.GetCurrentHand(PhotonNetwork.player.ID);
            ActionCallBack();
        }
        if (PhotonNetwork.player.ID == cont[0])
        {
             hand.GetCurrentHand(PhotonNetwork.player.ID);
        }

        //bool request = (bool) content; //TODO shareknowledge or not

    }

    //end shareknowledge event

    
    public void InfectCityByCard(InfectionCard card)
    {
        GameObject cityObject = GameObject.Find(card.ToString());
        City cityScript = cityObject.GetComponent<City>();
        cityScript.InfectByCard(card);
    }

    public void initializeDiseaseCubesForCity(InfectionCard card)
    {
        GameObject foreground = GameObject.Find("Foreground");
        float x1Offset = 5;
        float y1Offset = 16;
        float x2Offset = -10;
        float y2Offset = 0;

        GameObject city = GameObject.Find(card.ToString());
        //int count = 0;
        //foreach (Transform child in foreground.transform) {
        //if(count < 48) {
        string test = city.name + "Blue";

        //If disease cubes for a city were already initialized, then return 
        if (GameObject.Find(test) != null)
        {
            return;
        }
        Vector3 cityPosition = city.transform.position;
        //For Blue Disease Cube
        Vector3 blueDiseaseCubePos = new Vector3(cityPosition.x + x1Offset, cityPosition.y + y2Offset, cityPosition.z);
        GameObject blue = Instantiate(blueDisease);
        blue.transform.position = blueDiseaseCubePos;
        blue.name = city.name + "Blue";
        blue.transform.parent = GameObject.Find("Foreground").transform;

        //For Red Disease Cube
        Vector3 redDiseaseCubePos = new Vector3(cityPosition.x + x2Offset, cityPosition.y + y2Offset, cityPosition.z);
        GameObject red = Instantiate(redDisease);
        red.transform.position = redDiseaseCubePos;
        red.name = city.name + "Red";
        red.transform.parent = GameObject.Find("Foreground").transform;

        //For Black Disease Cube
        Vector3 blackDiseaseCubePos = new Vector3(cityPosition.x + x2Offset, cityPosition.y + y1Offset, cityPosition.z);
        GameObject black = Instantiate(blackDisease);
        black.transform.position = blackDiseaseCubePos;
        black.name = city.name + "Black";
        black.transform.parent = GameObject.Find("Foreground").transform;

        //For Yellow Disease Cube
        Vector3 yellowDiseaseCubePos = new Vector3(cityPosition.x + x1Offset, cityPosition.y + y1Offset, cityPosition.z);
        GameObject yellow = Instantiate(yellowDisease);
        yellow.transform.position = yellowDiseaseCubePos;
        yellow.name = city.name + "Yellow";
        yellow.transform.parent = GameObject.Find("Foreground").transform;




    }

    public void OnClickTreatDisease(int DiseaseType)
    {
        ActionButton.gameObject.SetActive(false);
        GameObject currentCity = currentPlayer.CurrentCity;
        City script = currentCity.GetComponent<City>();
        int[] content = { script.cityNumber, DiseaseType };


        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };

        if (this.currentPlayer.PlayerRole == Role.Medic)
        {
            for (int i = 0; i < 3; i++)
            {
                PhotonNetwork.RaiseEvent((byte)121, content, true, opt);
            }
        }
        else
        {
            PhotonNetwork.RaiseEvent((byte)121, content, true, opt);
        }
       
        ActionCallBack();
    }

    public void TreatDiseaseEvent(byte evCode, object content, int senderId)
    {
        if ((byte)121 != evCode)
        {
            return;
        }
        int[] cont = content as int[];
        TreatDisease((PlayerCard)cont[0], cont[1]);

    }

    public void TreatDisease(PlayerCard city, int DiseaseType)
    {
        GameObject currentCity = GameObject.Find(city.ToString());
        City script = currentCity.GetComponent<City>();
        if (DiseaseType == 1)
        {
            int redDiseaseCount = script.getRedDisease();
            if (redDiseaseCount > 0)
            {
                script.setRedDisease();
                redDiseaseCount -= 1;
                string temp = currentCity.name + "Red";
                GameObject diseaseCube = GameObject.Find(temp);
                diseaseCube.GetComponent<TextMeshProUGUI>().text = redDiseaseCount.ToString();
            }
        }
        else if(DiseaseType == 2)
        {
            int blueDiseaseCount = script.getBlueDisease();
            if (blueDiseaseCount > 0)
            {
                script.setBlueDisease();
                blueDiseaseCount -= 1;
                string temp = currentCity.name + "Blue";
                GameObject diseaseCube = GameObject.Find(temp);
                diseaseCube.GetComponent<TextMeshProUGUI>().text = blueDiseaseCount.ToString();
            }
        }
        else if(DiseaseType == 3)
        {
            int yellowDiseaseCount = script.getYellowDisease();
            if (yellowDiseaseCount > 0)
            {
                script.setYellowDisease();
                yellowDiseaseCount -= 1;
                string temp = currentCity.name + "Yellow";
                GameObject diseaseCube = GameObject.Find(temp);
                diseaseCube.GetComponent<TextMeshProUGUI>().text = yellowDiseaseCount.ToString();
            }
        }
        else if(DiseaseType == 4)
        {
            int blackDiseaseCount = script.getBlackDisease();
            if (blackDiseaseCount > 0)
            {
                script.setBlackDisease();
                blackDiseaseCount -= 1;
                string temp = currentCity.name + "Black";
                GameObject diseaseCube = GameObject.Find(temp);
                diseaseCube.GetComponent<TextMeshProUGUI>().text = blackDiseaseCount.ToString();
            }
        }
    }

    

    public void OnClickDrive()
    {
        if (!cityManager.AreNeighbors(currentPlayer.CurrentCity, ActionButton.CurrentCity))
        {
            return; //TODO ADD WARING MESSAGE
        }
        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        ActionButton.gameObject.SetActive(false);
        if (currentPlayer.PlayerRole == Role.Medic)
        {
            GameObject currentCity = currentPlayer.CurrentCity;
            City script = currentCity.GetComponent<City>();
            int[] content = { script.cityNumber, 0 };
            if (redCureFlag) // RBY BLACK
            {
                content[1] = 1;
                for (int i = 0; i<3; i++)
                {
                    PhotonNetwork.RaiseEvent((byte)121, content, true, opt);
                }
            }
            else if (blueCureFlag) // RBY BLACK
            {
                content[1] = 2;
                for (int i = 0; i < 3; i++)
                {
                    PhotonNetwork.RaiseEvent((byte)121, content, true, opt);
                }
            }
            else if (yellowCureFlag) // RBY BLACK
            {
                content[1] = 3;
                for (int i = 0; i < 3; i++)
                {
                    PhotonNetwork.RaiseEvent((byte)121, content, true, opt);
                }
            }
            else if (blackCureFlag) // RBY BLACK
            {
                content[1] = 4;
                for (int i = 0; i < 3; i++)
                {
                    PhotonNetwork.RaiseEvent((byte)121, content, true, opt);
                }
            }
        }
        PhotonNetwork.RaiseEvent((byte)110, ActionButton.CurrentCity.name, true, opt);
        ActionCallBack();
        ActionButton.gameObject.SetActive(false);

    }

    private void DriveEvent (byte evCode, object content, int senderId) {
        //Debug.LogError("ShareKnowledgeEvent" + PhotonNetwork.player.ID);
        if (evCode != (byte) 110) {
            return;
        }
        Debug.LogError ("senderid" + senderId);
        players[senderId - 1].MoveTo (GameObject.Find ((string) content));
    }

    public void OnClickDirectFlight () {
        var playerhand = currentPlayer.GetPlayerHand ();
        bool hasCard = false;
        foreach (PlayerCard card in playerhand) {
            if (GameObject.Find (card.ToString ()).Equals (ActionButton.CurrentCity)) {
                currentPlayer.Discard (card);
                deck.PutInDiscardPile (card);
                hasCard = true;
                break;
            }
        }
        if (!hasCard) {
            return; //add error message
        }

        RaiseEventOptions opt = new RaiseEventOptions {
            Receivers = ReceiverGroup.All
        };
        //PhotonNetwork.RaiseEvent ((byte) 111, ActionButton.CurrentCity.GetComponent<City>().cityNumber, true, opt);
        PhotonNetwork.RaiseEvent((byte)110, ActionButton.CurrentCity.name, true, opt);
        PhotonNetwork.RaiseEvent((byte)77, ActionButton.CurrentCity.GetComponent<City>().cityNumber, true, opt);
        ActionCallBack();
        ActionButton.gameObject.SetActive (false);
    }

    private void PlayerDiscardEvent(byte evCode, object content, int senderId)
    {
        if (evCode != (byte)77)
        {
            return;
        }
        players[senderId - 1].Discard((PlayerCard)content);
        deck.PutInDiscardPile((PlayerCard)content);
        if (PhotonNetwork.player.ID == senderId)
        {
            hand.ShowHand();
        }
    }

    private void DirectFlightEvent (byte evCode, object content, int senderId) {
        if (evCode != (byte) 111) {
            return;
        }
        //Debug.LogError ("senderid" + senderId);
        players[senderId - 1].MoveTo (GameObject.Find (((PlayerCard)content).ToString()));
        players[senderId - 1].Discard ((PlayerCard)content);
        if (PhotonNetwork.player.ID == senderId) {
            hand.ShowHand ();
        }
    }

 
        public void ActionCallBack()
    {
        ActionTook++;
        Debug.Log(ActionTook);
        if (ActionTook == MaxAction)
        {
            
            PhotonNetwork.RaiseEvent((byte)122,null,true, new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            });
            int nextPlayer = PhotonNetwork.player.ID + 1;
            //Debug.LogError("PhotonNetwork.room.PlayerCount " + PhotonNetwork.room.PlayerCount);
            if (nextPlayer > PhotonNetwork.room.PlayerCount)
            {
                nextPlayer = 1;
            }
            GivePlayerTurn(nextPlayer);
            //GiveNextPlayerTurn();
            //EndTurn();
        }
    }
    
    int InfectionRate = 0;//index of infection rate
    int[] InfectionDrawCard = { 2, 2, 2, 3, 3, 4, 4 };

    private void EndTurn(int playerID)
    {
        //Debug.Log(ActionTook);
        Player player = players[playerID-1];
        for (int i = 0; i < 2; i++)
        {
            PlayerCard temp = deck.DrawPlayerCard();
            if (temp == PlayerCard.Epidemic)
            {
                EpidemicActivate();
            }
            player.AddToHand(temp);
        }
        


        for (int j = 0; j < InfectionDrawCard[InfectionRate]; j++)
        {
            InfectionCard temp = deck.DrawInfectionCard();
            deck.PutInDiscardPile(temp);
            //Initialize disease cube counters for city
            City city = GameObject.Find(temp.ToString()).GetComponent<City>();
            if (city.GetDiseaseNumber(city.DiseaseType)==3){
                OutBreak(city.gameObject, city.DiseaseType);
            }
            else
            {
                initializeDiseaseCubesForCity(temp);
                InfectCityByCard(temp);
            }

            
            
        }


        //GiveNextPlayerTurn();
    }

    public void testOutbreak()
    {
        GameObject city = GameObject.Find("Santiago");
        //OutBreak(city, "bluedisease");
        City curCity = city.GetComponent<City>();
        EpidemicActivate();
        //curCity.OutbreakFlag = true;
    }

    private void OutBreak(GameObject city, string type)
    {
        City curCity = city.GetComponent<City>();
        List<GameObject> Neighbors = cityManager.GetNeighbors(city);
        
        if (curCity.OutbreakFlag == true)
        {
            return;
        }
        
        curCity.OutbreakFlag = true;
        foreach(GameObject City in Neighbors)
        {
            City neighbor = City.GetComponent<City>();
            InfectionCard temp = (InfectionCard)neighbor.cityNumber;
            Debug.Log(neighbor.gameObject.name + " outbreak " + neighbor.OutbreakFlag);

            if (neighbor.GetDiseaseNumber(type) == 3)
            {
                OutBreak(neighbor.gameObject, type);
            }
            else
            {
                initializeDiseaseCubesForCity(temp);
                neighbor.InfectCity(type);
            }
            
        }
        curCity.OutbreakFlag = false;

    }

    public void EpidemicActivate()
    {
        infectionRate++;
        InfectionCard card = deck.RemoveFromBottom();
        for(int i = 0; i < 3; i++)
        {
            InfectCityByCard(card);
            initializeDiseaseCubesForCity(card);
        }

        deck.DiscardPileOnTop();
    }

    private void EndTurnEvent (byte evCode, object content, int senderId) {
        if (evCode != (byte)122)
        {
            return;
        }
        EndTurn(senderId);
        if (senderId == PhotonNetwork.player.ID)
        {
            hand.GetCurrentHand(senderId);
        }
    }

    private void GiveNextPlayerTurn()
    {
        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others
        };
        ActionTook = 0;
        isYourTurn = false;
        ActionButton.disable = true;
        PhotonNetwork.RaiseEvent((byte)120, null, true, opt);
    }

    private void GivePlayerTurn(int id)
    {
        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        
        PhotonNetwork.RaiseEvent((byte)130, id, true, opt);
    }

    private void GiveTurnEvent(byte evCode, object content, int senderId)
    {
        if (evCode != (byte)130)
        {
            return;
        }
        int id = (int)content;
        Debug.Log("give turn "+id);
        if (PhotonNetwork.player.ID == id)
        {
            ActionTook = 0;
            isYourTurn = true;
            ActionButton.disable = false;
        }
        else
        {
            isYourTurn = false;
            ActionButton.disable = true;
        }


    }

    private void TurnEvent(byte evCode, object content, int senderId)
    {
        if (evCode != (byte)120)
        {
            return;
        }
        Debug.LogError("senderid "+senderId);
        int nextPlayer = senderId + 1;
        Debug.LogError("PhotonNetwork.room.PlayerCount " + PhotonNetwork.room.PlayerCount);
        if (nextPlayer > PhotonNetwork.room.PlayerCount)
        {
            nextPlayer = 1;
        }
        if (PhotonNetwork.player.ID != nextPlayer)
        {
            return;
        }
        isYourTurn = true;
        ActionButton.disable = false;
    }

        void Start () {
        //PhotonNetwork.OnEventCall += this.OnEventHandler;
        //PhotonNetwork.EventCallback(0,null,1);
        PhotonNetwork.OnEventCall += this.testCityhandler;
        PhotonNetwork.OnEventCall += this.DistributeCardEvent;
        PhotonNetwork.OnEventCall += this.ShuffleCardEvent;
        PhotonNetwork.OnEventCall += this.InitializeEvent;
        PhotonNetwork.OnEventCall += this.DirectFlightEvent;
        PhotonNetwork.OnEventCall += this.DriveEvent; 
        PhotonNetwork.OnEventCall += this.ShareKnowledgeEvent;
        PhotonNetwork.OnEventCall += this.ShareKnowledgeRequestEvent;
        PhotonNetwork.OnEventCall += this.GiveTurnEvent;
        PhotonNetwork.OnEventCall += this.TreatDiseaseEvent;
        PhotonNetwork.OnEventCall += this.EndTurnEvent;
        PhotonNetwork.OnEventCall += this.ResearchStationEvent;
        PhotonNetwork.OnEventCall += this.NewShareKnowledgeRequestEvent;
        PhotonNetwork.OnEventCall += this.PlayerDiscardEvent;


          InfectionCard tmp = deck.DrawInfectionCard();
        initializeDiseaseCubesForCity(tmp);
        InfectCityByCard(tmp);

        int PlayerNumber = PhotonNetwork.player.ID;
        switch (PlayerNumber) {
            case 1:
                currentPlayer = player1;
                break;
            case 2:
                currentPlayer = player2;
                break;
            case 3:
                currentPlayer = player3;
                break;
            default:
                gameObject.SetActive (false);
                return;
        }

        players[0] = player1;
        players[1] = player2;
        players[2] = player3;



        if (PhotonNetwork.isMasterClient) {
            //GiveNextPlayerTurn();
            GivePlayerTurn(1);
            RaiseEventOptions opt = new RaiseEventOptions {
                Receivers = ReceiverGroup.All
            };
            System.Random rnd = new System.Random ();
            int seed = rnd.Next ();
            PhotonNetwork.RaiseEvent ((byte) 99, seed, true, opt);
            //deck.ShufflePlayerCard (seed);

            //PhotonNetwork.RaiseEvent ((byte) 101, null, true, opt);

        }
        int playerinroom = PhotonNetwork.room.PlayerCount;
        RectTransform rect = PlayerButtons.GetComponent<RectTransform> ();
        Debug.Log (playerinroom);
        while (playerinroom < 4) {
            rect.GetChild (playerinroom++).gameObject.SetActive (false);
            //Debug.Log (rect.GetChild (playerinroom).gameObject.name);
        }

    }

    public GameObject ResearchStation;
    public void OnClickReseashStation()
    {

        ActionButton.gameObject.SetActive(false);
        City CurrentCity = (City)ActionButton.CurrentCity.GetComponent("City");
        PlayerCard card = (PlayerCard)CurrentCity.cityNumber;
        RaiseEventOptions opt = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        if (CurrentCity.HasResearchStation())
        {
            return;//if city has research station already then exit
        }

        if (currentPlayer.PlayerRole == Role.OperationsExpert)
        {
            PhotonNetwork.RaiseEvent((byte)123, card.GetHashCode(), true, opt);
            ActionCallBack();
            return;
        }

        if (!currentPlayer.HasCard(card)||!currentPlayer.CurrentCity.Equals(ActionButton.CurrentCity) )
        {
            return;//if player does not have card required to build research station then exit
        }
        currentPlayer.Discard(card);
        deck.PutInDiscardPile(card);
        hand.GetCurrentHand(PhotonNetwork.player.ID);
        //CurrentCity.SetResearchStation(PlaceResearchStation(ActionButton.CurrentCity, ResearchStation));

        //Instantiate(ResearchStation)
        
        PhotonNetwork.RaiseEvent((byte)123, card.GetHashCode(),true,opt);
        ActionCallBack();
    }

    private void ResearchStationEvent(byte evCode, object content, int senderId)
    {
        Debug.LogError("ResearchStationEvent "+PhotonNetwork.player.ID);
        if (evCode != (byte)123)
        {
            return;
        }
        int cont = (int)content;
        PlayerCard card = (PlayerCard)cont;
        GameObject City = GameObject.Find(card.ToString());
        City CurrentCity = City.GetComponent<City>();
        CurrentCity.SetResearchStation(PlaceResearchStation(City, ResearchStation));
    }
        public GameObject PlaceResearchStation(GameObject city, GameObject ResearchStation)
    {
        Vector3 location = city.transform.position;
        location.y += 15;
        GameObject researchStation = Instantiate(ResearchStation, location, Quaternion.identity) as GameObject;
        researchStation.name = city.name + "RS";
        researchStation.transform.parent = GameObject.Find("Canvas1").transform;
        //Deduce from research station counter here
        return researchStation;
    }

    public void RemoveResearchStation(GameObject researchStation)
    {
        Destroy(researchStation);
        //increment research station counter here
    }

    private int infectionRate;
    




    // Update is called once per frame
    void Update () {

    }
}