using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sendRequest : Photon.PunBehaviour
{

	public Button acc;
	public Button rej;
	public Text requestMsg;
    private int Request;
    public GameManager gamemanager;
    private PlayerCard CityCard;
    public int[] cont; 

    

    public int GetRequest()
    {
        return Request;
    }

    public PlayerCard GetCityCardChoose()
    {
        return CityCard;
    }

    public void ChangeRequestMsg(string Msg)
    {
        requestMsg.text = Msg;
    }

    public void ShowMenu(string Msg)
    {
        
        rej.gameObject.SetActive(true);
        acc.onClick.AddListener(() => ButtonResponseRequest(acc));
        rej.onClick.AddListener(() => ButtonResponseRequest(rej));
        Request = -1;
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
    }

    public void ShowChooseShareCardMenu(string Msg, PlayerCard card)
    {
        rej.gameObject.SetActive(true);
        acc.onClick.AddListener(() => ButtonResponseRequest(acc,card));
        rej.onClick.AddListener(() => ButtonResponseRequest(rej));
        Request = -1;
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
    }

    public void ShowChooseCityCardMenu(string Msg, PlayerCard card)
    {
        rej.gameObject.SetActive(true);
        acc.onClick.AddListener(() => ButtonResponseRequest(acc, card));
        rej.onClick.AddListener(() => ButtonResponseRequest(rej));
        Request = -1;
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
    }

    private int EventType;

    public void ShowMenu(string Msg, int eventType)
    {
        EventType = eventType;

        rej.gameObject.SetActive(true);
        acc.onClick.AddListener(() => ButtonResponseRequest(acc));
        rej.onClick.AddListener(() => ButtonResponseRequest(rej));
        Request = -1;
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
    }

    public void ShowMenu(string Msg, int[] content)
    {
        string debug = "";
        for (int i = 0; i < 4; i++)
        {
            debug += content[i].ToString() + " ";
        }
        Debug.Log("in ShareKnoledge " + debug);

        rej.gameObject.SetActive(true);
        acc.onClick.AddListener(() => ButtonResponseRequest(acc, content));
        rej.onClick.AddListener(() => ButtonResponseRequest(rej));
        Request = -1;
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
    }

    public void ShowMenu(string Msg, int a, int b, int c, int d)
    {

        rej.gameObject.SetActive(true);
        acc.onClick.AddListener(() => ButtonResponseRequest(acc, a,b,c,d));
        rej.onClick.AddListener(() => ButtonResponseRequest(rej));
        Request = -1;
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
    }

    private void ButtonResponseRequest(Button buttonPressed, int a, int b, int c, int d)
    {
        if (buttonPressed == acc)
        {
            cont[0] = a;
            cont[1] = b;
            cont[2] = c;
            cont[3] = d;
            Request = 1;
            Debug.Log(GetRequest());
        }
        else if (buttonPressed == rej)
        {
            Debug.Log("false");
            Request = 0;
        }
        OnClose();
        gameObject.SetActive(false);
    }


    public void ShowMessage(string Msg)
    {
        acc.onClick.AddListener(() => gameObject.SetActive(false));
        ChangeRequestMsg(Msg);
        gameObject.SetActive(true);
        rej.gameObject.SetActive(false);
    }

    

    private void ButtonResponseRequest(Button buttonPressed){
        if (buttonPressed == acc)
        {
            if (EventType == Role.OperationsExpert.GetHashCode())
            {
                gamemanager.OnOperationsExpertAbility = true;
            }
            Request = 1;
            Debug.Log(GetRequest());
        }
        else if (buttonPressed == rej)
        {
            Debug.Log("false");
            Request = 0;
        }
        OnClose();
        gameObject.SetActive(false);
    }

    private void ButtonResponseRequest(Button buttonPressed, PlayerCard card)
    {
        if (buttonPressed == acc)
        {
            CityCard = card;
            Request = 1;
            //Debug.Log(GetRequest());
        }
        else if (buttonPressed == rej)
        {
            //Debug.Log("false");
            Request = 0;
        }
        OnClose();
        gameObject.SetActive(false);
    }

    private void ButtonResponseRequest(Button buttonPressed, int[] content)
    {
        string debug = "";
        for (int i = 0; i < 4; i++)
        {
            debug += content[i].ToString() + " ";
        }
        Debug.Log("in ButtonResponseRequest " + debug);


        if (buttonPressed == acc)
        {

            content[3] = 1;
            Request = 1;
            
            //Debug.Log(GetRequest());
        }
        else if (buttonPressed == rej)
        {
            //Debug.Log("false");
            content[3] = 0;
            Request = 0;
        }
        //Debug.Log("111");
        //this.cont = content;
        cont = new int[4];
        for (int i = 0; i < 4; i++)
        {
            cont[i] = content[i];
        }
        OnClose();
        gameObject.SetActive(false);
    }

    void OnEnable(){
        CityCard = PlayerCard.Empty;
        //cont = null;
        cont = new int[4];
        cont[0] = -1;
    }

    void OnClose()
    {
        string debug = "";
        for (int i = 0; i < 4; i++)
        {
            debug += cont[i].ToString() + " ";
        }
        Debug.Log("in OnClose " + debug);

        if (CityCard != PlayerCard.Empty)
        {
            gamemanager.SelectCardCallBack(CityCard);
            CityCard = PlayerCard.Empty;
        }

        if (EventType == 1)//enent type 1 send request back
        {
            if (Request == -1)
            {
                Debug.LogError("should not be -1");
            }
            gamemanager.RequestCallback(Request == 1);
        }
        EventType = 0;
        //if (cont[3] != -1)//change
        //{

           // gamemanager.RequestCallback(cont);
           // cont = null;
       // }
        
    }

	void OnDisable(){
		acc.onClick.RemoveAllListeners();
    	rej.onClick.RemoveAllListeners();
        Request = -1;
        //Debug.Log(CityCard);
        
    }


}
