using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinGameLobby : Photon.PunBehaviour {
	public GameObject roomMessagePanel;
	// Use this for initialization
	void Start () {
		//displayRooms();
		ShowRoomMessage ();
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		string username = PlayerPrefs.GetString ("username");
		hashtable.Add ("username", username);
		PhotonNetwork.player.SetCustomProperties (hashtable);

	}

	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 10 == 0) { ShowRoomMessage (); }
	}

	// public void displayRooms () {
	// 	List<string> roomNames = new List<string> ();
	// 	RoomInfo[] roomInfos = PhotonNetwork.GetRoomList ();
	// 	//for(int i = 1; i <= 4; i++) {
	// 	foreach (RoomInfo info in roomInfos) {
	// 		//roomNames.Add (info.name);
	// 		roomNames.Add (info.Name);
	// 		Debug.Log (info);
	// 		//}
	// 	}

	// 	for (int i = 1; i <= roomNames.Count; i++) {
	// 		GameObject.Find ("Room" + i).GetComponentInChildren<Text> ().text = roomNames[i - 1];
	// 	}
	// }
	public void ShowRoomMessage () {
		//int start, end, i, j;
		int i, j, end = 0;
		// start = (currentPageNumber - 1) * roomPerPage; //计算需要显示房间信息的起始序号
		// if (currentPageNumber * roomPerPage < roomInfo.Length) //计算需要显示房间信息的末尾序号
		// 	end = currentPageNumber * roomPerPage;
		// else
		// 	end = roomInfo.Length;
		GameObject[] roomMessage = new GameObject[4];
		RectTransform rectT = roomMessagePanel.GetComponent<RectTransform> ();
		RoomInfo[] roomInfo = PhotonNetwork.GetRoomList ();
		foreach (var item in roomInfo)
		{
			end ++;
		}
		for (i = 0; i < 4; i++) {
			roomMessage[i] = rectT.GetChild (i).gameObject;
			roomMessage[i].SetActive (false);
		}
		//display room message

		for (i = 0, j = 0; i < end; i++, j++) {
			RectTransform rectTransform = roomMessage[i].GetComponent<RectTransform> ();
			string roomName = roomInfo[i].Name; //get room name
			rectTransform.GetChild (0).GetComponent<Text> ().text = (i + 1).ToString (); //room number
			rectTransform.GetChild (1).GetComponent<Text> ().text = roomName;
			rectTransform.GetChild (2).GetComponent<Text> ().text = roomInfo[i].PlayerCount + "/" + roomInfo[i].MaxPlayers; //display capacity
			Button button = rectTransform.GetChild (3).GetComponent<Button> (); //get button
			//disable join room button if room is full or game is already played
			if (roomInfo[i].PlayerCount == roomInfo[i].MaxPlayers || roomInfo[i].IsOpen == false)
				button.gameObject.SetActive (false);
			//give room button function if room can join
			else {
				button.gameObject.SetActive (true);
				button.onClick.RemoveAllListeners ();
				button.onClick.AddListener (delegate () {
					ClickJoinRoomButton (roomName);
				});
			}
			roomMessage[j].SetActive (true); //set this room message active
		}
		//disable room message not use
		while (j < 4) {
			roomMessage[j++].SetActive (false);
		}
	}

	public void ClickJoinRoomButton (string roomName) {
		PhotonNetwork.JoinRoom (roomName);
		//roomLoadingLabel.SetActive (true); 
	}
	public void takeToRoom1 () {
		PhotonNetwork.JoinRoom (GameObject.Find ("Room1").GetComponentInChildren<Text> ().text);
		Debug.Log ("Joined Room " + GameObject.Find ("Room1").GetComponentInChildren<Text> ().text);
	}

	public void takeToRoom2 () {
		PhotonNetwork.JoinRoom (GameObject.Find ("Room2").GetComponentInChildren<Text> ().text);
		Debug.Log ("Joined Room " + GameObject.Find ("Room2").GetComponentInChildren<Text> ().text);
	}

   public override void OnLeftLobby()
    {
        PhotonNetwork.LoadLevel(1);
    }

}