using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextLocalization : MonoBehaviour {
	public string key;

	// Use this for initialization
	void Start () {
		Text text = GetComponent<Text> ();
		if (text != null) {
			text.text = LangUtils.Get(key);
		}
	}

}
