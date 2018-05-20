using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : Photon.PunBehaviour {

	/// <summary>
	///  This client's version number. Users are separated from each other by game version.
	/// </summary>
	string _gameVersion = "1";

	// This is for testing (to get the number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created.

//	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created")]
//	public byte MaxPlayersPerRoom = 4;

	// The PUN loglevel

	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

	/// <summary>
	/// MonoBheaviour method called on GameObject by Unity during early initialization phase.
	/// </summary>
	// Use this for initialization

	void Awake() {

		// #Critical
		//[DEPRECATED COMMENT.] We don't join the lobby. There is no need to join a lobby to get the list of rooms.

		PhotonNetwork.autoJoinLobby = true;

		// Force Loglevel
		PhotonNetwork.logLevel = Loglevel;

		// #Critical
		// This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically.
		PhotonNetwork.automaticallySyncScene = true;
	}

	/// <summary>
	/// MonoBheaviour method called on GameObject by Unity during initialization phase.
	/// </summary>
	void Start () {
		Connect ();
	}

	/// <summary>
	/// Start the connection process.
	/// - If not connected, Connect this application instance to Photon Cloud Network.
	/// </summary>

	public void Connect()
	{
		if(!PhotonNetwork.connected){
			Debug.Log ("Connecting to server...");
			PhotonNetwork.ConnectUsingSettings(_gameVersion);

		
		}
	}
		
	// Update is called once per frame
	void Update () {
		
	}

	//This method will take the user to the New Game Lobby after clicking on Create New Game
	public void takeToNewGameLobby() {
		PhotonNetwork.LoadLevel("NewGameLobby");
	}

	//This method will take the user to the Join Game Lobby after clicking on Join Game
	public void takeToJoinGameLobby() {
		PhotonNetwork.LoadLevel ("JoinGameLobby");
	}
		
}

//ONLY HAVE THIS HERE TO BRING UP RPSCORE AND PUNTURNMANAGER
//public interface IPunTurnManagerCallbacks
//{
	
//}
