using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccountShow : MonoBehaviour {

	public Scope scope;

	void Start () {
		OnScopeChange ();
	}
	
	// Update is called once per frame
	public void OnScopeChange() {
		string name = scope.Query<string> ("name");
		if (name != null) {
			GetComponent<Text> ().text = name;
		}
	}
}
