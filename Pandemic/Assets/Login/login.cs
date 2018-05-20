using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login : MonoBehaviour {

	public InputField email;
	public InputField password;

	private String mail;
	private String pass;
	private int err = 0;
	private AssetBundle myLoadedAssetBundle;
	private string[] scenePaths;
	// Use this for initialization
	void Start () {

		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent ();
		submitEvent.AddListener (SubmitLoginInfo);
		email.onEndEdit = submitEvent;
		password.onEndEdit = submitEvent;

		// email.characterValidation = InputField.CharacterValidation.Alphanumeric;
		// password.characterValidation = InputField.CharacterValidation.Alphanumeric;

		//	myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
		//  scenePaths = myLoadedAssetBundle.GetAllScenePaths();
	}

	public void SubmitLoginInfo (String name) {
		mail = email.text;
		pass = password.text;
	}

	void setPlayerpref (String email) {
		string usernameQuery = "select username from accounts WHERE Email = '" + email + "';";
		DataSet dgv = MySqlHelper.GetDataSet (usernameQuery, null);
		string username = (string) dgv.Tables[0].Rows[0][0];
		PlayerPrefs.SetString ("username", username);
		PlayerPrefs.SetString ("email", email);
	}
	void Login (String email_in, String password_in) {
		//SceneManager.LoadScene ("MainMenu");
		string findEmail = "select password from accounts WHERE Email = '" + email_in + "';";
		DataSet dgv = MySqlHelper.GetDataSet (findEmail, null);
		if (dgv.Tables[0].Rows.Count == 0) {
			err = 1;
		} else {
			string pwd = (string) dgv.Tables[0].Rows[0][0];
			if (pwd.Equals (password_in)) {
				//login. Go to new scene.
				//	Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);
				err = 3;
				email.text = "";
				password.text = "";

				Debug.Log ("success");
				err = 0;
				setPlayerpref (email_in);
				SceneManager.LoadScene ("MainMenu");

			} else {
				err = 2;
				Console.WriteLine ("fail");
				//password not matchlogin fail
			}
		}
	}

	public void LoginOnClick () {
		if (mail != "" && pass != "") {
			Login (mail, pass);
		} else {
			err = 4;
		}
	}

	void OnGUI () {
		if (err == 0) { } else if (err == 1) {

			GUI.Label (new Rect (700, 100, 400, 200), "There is no account associated with this email. Please try again.");

		} else if (err == 2) {

			GUI.Label (new Rect (700, 100, 400, 200), "That is not the correct password. Please try again.");

			//		}else if(err==3){
			//			SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Single);
		} else if (err == 4) {

			GUI.Label (new Rect (700, 100, 400, 200), "Please make sure provide all information.");

		}
	}

}