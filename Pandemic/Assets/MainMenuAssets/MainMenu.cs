using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public GameObject Mainmenu;
	public GameObject CreateRoom;
	public void QuitGame () {

		Application.Quit ();

	}
	public void OnClickCancel () {
		CreateRoom.SetActive (false);
		Mainmenu.SetActive (true);

	}

	public void OnClickCreateNewGame () {
		Mainmenu.SetActive (false);
		CreateRoom.SetActive (true);
	}
}