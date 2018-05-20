using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameLobbyManager : Photon.PunBehaviour {

	/// <summary>
	/// Called when the local player leaves the room. We need to load the MainMenu scene.
	/// </summary>

	public override void OnLeftRoom()
	{
		PhotonNetwork.LoadLevel (1);
	}

	#region Photon.PunBehaviour CallBacks

	// DEPRECATED AT THIS POINT
	public override void OnConnectedToMaster()
	{
		Debug.Log ("DemoAnimator/MainMenu: OnConnectedToMaster() was called by PUN");
		// Create the new room
		//PhotonNetwork.JoinLobby();
		if (PhotonNetwork.inRoom) {
			//OnJoinedRoom ();
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			string username = PlayerPrefs.GetString ("username");
			hashtable.Add ("username", username);
			PhotonNetwork.player.SetCustomProperties (hashtable);

		}

	}

	public override void OnDisconnectedFromPhoton()
	{
		Debug.LogWarning ("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
	}

	#endregion


	/*public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		Debug.Log ("DemoAnimator/NetworkManager:OnPhotonRandomJoinFailed() was called by PUN. No random room available, we create one. \n Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		// Create the new room
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
	}*/


	void LoadArena()
	{
		if (!PhotonNetwork.isMasterClient) {
			Debug.LogError ("PhotonNetwork : Trying to Load a level but we are not the master Client");
		}
		Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
		PhotonNetwork.LoadLevel("GameBoard");
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom ();
	}

	public void SetReadyUp()
	{
		Debug.Log(PhotonNetwork.room.CustomProperties.ToString());
		ExitGames.Client.Photon.Hashtable hashtable = PhotonNetwork.room.CustomProperties;
		int maxPlayers = (int) hashtable ["maxPlayers"];

		if (PhotonNetwork.room.PlayerCount == maxPlayers && PhotonNetwork.isMasterClient) {
			// Testing loading game for 1 person
			Debug.Log("We load the game for everyone");
			// #Critical
			// Load the Room Level.
			LoadArena();
		}
	}


	///<summary>
	/// Currently not being used.
	/// </summary>

	/*public override void OnJoinedRoom()
	{
		Debug.Log ("DemoAnimator/NetworkManager: OnJoinedRoom() called by PUN. Now this client is in a room.");
		Debug.Log ("There are " + PhotonNetwork.countOfRooms + " rooms");
		if (ready == true) {
			// Testing loading game for 1 person
			Debug.Log("We load the game for everyone");
			// #Critical
			// Load the Room Level.
			LoadArena();
		}
	}*/



	/*public override void OnPhotonPlayerConnected(PhotonPlayer other)
	{
		//Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting
		Debug.Log("You are not MasterClient");

		if (PhotonNetwork.isMasterClient) 
		{
			Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


			LoadArena();
		}
	}*/


	/*public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
	{
		//Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


		if (PhotonNetwork.isMasterClient) 
		{
			Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


			LoadArena();
		}
	}*/
		


	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
