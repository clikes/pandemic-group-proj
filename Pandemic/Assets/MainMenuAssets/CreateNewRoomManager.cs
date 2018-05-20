using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewRoomManager : MonoBehaviour {
	public InputField RoomName;
	public Text Warning;

	public void OnCreateNewRoom () {
		RoomOptions option = new RoomOptions () { MaxPlayers = 4 };

        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        int maxPlayers = GameObject.Find("Settings").GetComponent<settings>().getNumberOfPlayers();
        int numberOfEpidemicCards = GameObject.Find("Settings").GetComponent<settings>().getNumberOfEpidemicCards();
        hashtable.Add("maxPlayers", maxPlayers);
        hashtable.Add("numberOfEpidemicCards", numberOfEpidemicCards);
        //PhotonNetwork.room.SetCustomProperties(hashtable);

        option.CustomRoomProperties = hashtable;

        RoomInfo[] roomInfos = PhotonNetwork.GetRoomList ();
		bool isRoomNameRepeat = false;
		foreach (RoomInfo info in roomInfos) {
			if (RoomName.text == info.Name) {
				isRoomNameRepeat = true;
				break;
			}
		}
		if (isRoomNameRepeat) {
			Warning.text = "Room name is already exist!";
		}
		else {
			PhotonNetwork.CreateRoom (RoomName.text, option, null); 
			
			PhotonNetwork.LoadLevel("NewGameLobby");

		}
	}
}