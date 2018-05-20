using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using System;
using System.Text.RegularExpressions;

public class register : MonoBehaviour {

	public InputField username = null;
	public InputField email=null;
	public InputField password1=null;
	public InputField password2=null;

	public GameObject log=null;
	public GameObject reg=null;

	private String user=null;
	private String mail=null;
	private String pass1=null;
	private String pass2=null;

	private int err=0;

private void Start(){
	InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
	submitEvent.AddListener(SubmitRegisterInfo);
	username.onEndEdit=submitEvent;
	email.onEndEdit=submitEvent;
	password1.onEndEdit=submitEvent;
	password2.onEndEdit=submitEvent;

//	username.characterValidation = InputField.CharacterValidation.Alphanumeric;
//	email.characterValidation = InputField.CharacterValidation.Alphanumeric;
//	password1.characterValidation = InputField.CharacterValidation.Alphanumeric;
//	password2.characterValidation = InputField.CharacterValidation.Alphanumeric;	
}

	void RegisterInToDB(string email, string username, string password){
		string[] value = {email,username,password};
		MySqlHelper.Insert("accounts",value);
	}

 public void SubmitRegisterInfo(String name)
     {
		 user=username.text;
		 mail=email.text;
		 pass1=password1.text;
		 pass2=password2.text;
	 }
public void printing(){
	Debug.Log(user);
}

public void Register(){
	if(user!="" && mail!="" && pass1!="" && pass2!=""){
		if(pass1==(pass2)){
						//check if the email is already in use.
			string findEmail = "select password from accounts WHERE Email = '" + mail + "';";
            DataSet dgv = MySqlHelper.GetDataSet(findEmail, null);
            //if email is not in use.
			if (dgv.Tables[0].Rows.Count==0){  
				Debug.Log(dgv.Tables[0].Rows.Count);
                RegisterInToDB(user, pass1, mail);
				err=0;
				username.text="";
				email.text="";
				password1.text="";
				password2.text="";
				log.SetActive(true);
				reg.SetActive(false);
				Debug.Log("inputted new user");
            }else{ //email is in use.			
				err=1;
				Debug.Log("Email already in use.");			
			}
		}else{
			err=2;
			Debug.Log("The passwords are no the same.");
		}	
	}else{
		err=3;
		Debug.Log("Missing fields for registering.");
	}
}

public void backOnClick(){
	email.text="";
	password1.text="";
	password2.text="";
	email.text="";
	log.SetActive(true);
	reg.SetActive(false);
}

void OnGUI(){
	if(err==0){
	}
	else if(err==1){
		String m ="Email already in use.";

                GUI.Label(new Rect(700, 100, 400, 200), m);

	}else if(err==2){
		String p = "The password and the confirmation password are not the same.";

                GUI.Label(new Rect(700, 100, 400, 200), p); //second one is y axis
            
	}else if(err==3){
            String r = "missing registering fields.";
                GUI.Label(new Rect(700, 100, 400, 200), r);
            
	}
}

}



