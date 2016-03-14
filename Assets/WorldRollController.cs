using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldRollController : MonoBehaviour {

	public List<string> msgs = new List<string>();
	public Text text;
	private bool rolling;
	
	void Update () {
		if (rolling) {
			return;
		}
		if (msgs.Count > 0) {
			Image bg = GetComponent<Image>();
			bg.enabled = true;
			rolling = true;
			Text msg = (Text)Instantiate (text);
			msg.gameObject.SetActive (true);
			msg.text = msgs [0];
			RectTransform rt = msg.GetComponent<RectTransform> ();
			rt.SetParent (transform);
			rt.localScale = text.transform.localScale;
			rt.localPosition = text.transform.localPosition;
			ContentSizeFitter fitter = msg.GetComponent<ContentSizeFitter> ();
			fitter.SetLayoutHorizontal ();
			msgs.RemoveAt (0);
		} else {
			Image bg = GetComponent<Image>();
			if(bg.enabled) {
				bg.enabled = false;
			}
		}
	}

	public void RollEnd() {
		rolling = false;
	}
}
