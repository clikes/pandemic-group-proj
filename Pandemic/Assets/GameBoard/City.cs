using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class City : MonoBehaviour {

	// Use this for initialization
	public GameObject playerActionButton;
    private int blueDisease;
    private int redDisease;
    private int yellowDisease;
    private int blackDisease;
    private int purpleDisease;
    private GameObject ResearchStation;
    public string DiseaseType;
    public int cityNumber;
    public bool OutbreakFlag = false;
    private GameManager gameManager;
    private bool isActive = false;
	void Start () {
		playerActionButton = GameObject.Find ("playerActionButton");
        blueDisease = 0;
        redDisease = 0;
        yellowDisease = 0;
        blackDisease = 0;
        purpleDisease = 0;
        ResearchStation = null;
        DiseaseType = gameObject.GetComponent<SpriteRenderer>().sprite.name;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
   
    public int GetDiseaseNumber(string diseaseType)
    {
        if (diseaseType == "bluedisease")
        {
            return blueDisease;
        }
        else if (diseaseType == "reddisease")
        {
            return redDisease;
        }
        else if (diseaseType == "yellowdisease")
        {
            return yellowDisease;
        }
        else if (diseaseType == "blackdisease")
        {
            return blackDisease;
        }
        else if (diseaseType == "purpledisease")
        {
            return purpleDisease;
        }
        else
        {
            return -1;
        }
        
    }

    public void SetResearchStation(GameObject ResearchStation)
    {
        this.ResearchStation = ResearchStation;
    }

    public bool HasResearchStation()
    {
        if(ResearchStation == null)
        {
            return false;
        }
        return true;
    }

    public void InfectCity(string diseaseType)
    {
        //Debug.Log (diseaseType);
        if (diseaseType == "bluedisease")
        {
            //Debug.Log("aaaa");
            blueDisease += 1;
            string stringBuilder = this.gameObject.name + "Blue";
            GameObject.Find(stringBuilder).GetComponent<TextMeshProUGUI>().text = blueDisease.ToString();
        }
        else if (diseaseType == "reddisease")
        {
            redDisease += 1;
            string stringBuilder = this.gameObject.name + "Red";
            GameObject.Find(stringBuilder).GetComponent<TextMeshProUGUI>().text = redDisease.ToString();
        }
        else if (diseaseType == "yellowdisease")
        {
            yellowDisease += 1;
            string stringBuilder = this.gameObject.name + "Yellow";
            GameObject.Find(stringBuilder).GetComponent<TextMeshProUGUI>().text = yellowDisease.ToString();
        }
        else if (diseaseType == "blackdisease")
        {
            blackDisease += 1;
            string stringBuilder = this.gameObject.name + "Black";
            GameObject.Find(stringBuilder).GetComponent<TextMeshProUGUI>().text = blackDisease.ToString();
        }
        else if (diseaseType == "purpledisease")
        {
            purpleDisease++;
        }
    }

    public void InfectByCard(InfectionCard card)
    {
        InfectCity(DiseaseType);
    }

    public void treatDisease(string diseaseType)
    {
        if (diseaseType == "bluedisease" && blueDisease > 0)
        {
            blueDisease--;
        }
        else if (diseaseType == "reddisease" && redDisease > 0)
        {
            redDisease--;
        }
        else if (diseaseType == "yellowdisease" && yellowDisease > 0)
        {
            yellowDisease--;
        }
        else if (diseaseType == "blackdisease" && blackDisease > 0)
        {
            blackDisease--;
        }
        else if (diseaseType == "purpledisease")
        {
            purpleDisease--;
        }
    }

    public void CureAllDisease()
    {
        blueDisease = 0;
        redDisease = 0;
        yellowDisease = 0;
        blackDisease = 0;
        purpleDisease = 0;
    }

    public int getRedDisease()
    {
        return redDisease;
    }

    // Decrement red disease count by 1
    public void setRedDisease()
    {
        redDisease -= 1;
    }

    public int getBlueDisease()
    {
        return blueDisease;
    }

    // Decrement blue disease count by 1
    public void setBlueDisease()
    {
        blueDisease -= 1;
    }

    public int getYellowDisease()
    {
        return yellowDisease;
    }

    // Decrement yellow disease count by 1
    public void setYellowDisease()
    {
        yellowDisease -= 1;
    }

    public int getBlackDisease()
    {
        return blackDisease;
    }

    // Decrement black disease count by 1
    public void setBlackDisease()
    {
        blackDisease -= 1;
    }


    void OnMouseDown () {
        //set current operation city
        ActionButtonManager script = playerActionButton.GetComponent<ActionButtonManager>();
        script.SetCurrentCity(GameObject.Find(name));
        if (gameManager.OnOperationsExpertAbility)
        {
            RaiseEventOptions opt = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent((byte)110, gameManager.OperationsExpertAbilityCard.ToString(), true, opt);
            PhotonNetwork.RaiseEvent((byte)77, gameManager.OperationsExpertAbilityCard.GetHashCode(), true, opt);
            gameManager.ActionCallBack();
            return;
        }
        if (script.disable)
        {
            return;
        }
        //getting the position of the object city clicked.
        Debug.Log(name);
		Vector3 pos = transform.position;
		pos.x += 70;
		pos.y += 40;
		Debug.Log (pos);
		if (playerActionButton.transform.position == pos) {
			if (playerActionButton.activeSelf == false) {
				playerActionButton.transform.position = pos;
				playerActionButton.SetActive (true);
			} else if (playerActionButton.activeSelf == true) {
				playerActionButton.SetActive (false);
			}
		} else {
			playerActionButton.transform.position = pos;
			playerActionButton.SetActive (true);
		}

	}

}