using UnityEngine;
using System.Collections;
using SimpleJson;

public class UserController : Scope {

	public GameObject backDrop;
	// Use this for initialization
	void Start () {
		backDrop.SetActive (true);
		JsonArray list = new JsonArray ();
		for (int i = 1; i <= 3; i++) {
			JsonObject json = new JsonObject ();
			json["ID"] = "andy";
			json["CountDown"] = "3h2m3d";
			json["Integral"] = 50;
			json["Flag"] = 1;
			list.Add (json);
		}
		Set ("ItemUser", list);
		Apply ();
		backDrop.SetActive (false);
	}

	public void OnUserSelectClick(string id) {
		UIManager.instance.LoadUI ("WinRank", null);
	}
	
}
