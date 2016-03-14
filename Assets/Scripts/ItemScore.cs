using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemScore : MonoBehaviour {

	public Scope scope;
	// Use this for initialization
	void Start () {
        
		scope.onScopeChange.AddListener (OnScopeChange);
		OnScopeChange ();
	}
	
	// Update is called once per frame
	public void OnScopeChange() {
       
		MyInfo myInfo = scope.Query<MyInfo> ("myInfo");
        
		if (myInfo != null) {
			GetComponent<Text> ().text = myInfo.score.ToString();
		}
	}
}
