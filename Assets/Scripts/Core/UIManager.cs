using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : Scope {

	public string defaultWin;
    public static string currentWin;
	public GameObject mainCanvas;

	private List<GameObject> uiList = new List<GameObject>();
	public static UIManager instance;
	public List<AudioClip> AC;
	public AudioSource music;
	
	void Awake () {
		instance = this;
		float bl = (float)Screen.width / Screen.height;
		if (bl < 1.4f) {
			CanvasScaler canvasScaler = mainCanvas.GetComponent<CanvasScaler>();
			canvasScaler.matchWidthOrHeight = 0f;
		}
		foreach(AudioClip a in AC) {
			Set(a.name, a);
		}
	}

	void Start() {
		LoadUI (defaultWin, null);
	}


	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.Escape)) {
			GoBack();
		}
        */
	}

	public void GoBack() {
		if (uiList.Count == 0) {
			Application.Quit();
		} else if (uiList.Count == 1) {
			if(uiList[0].name == defaultWin) {
				Application.Quit();
			} else {
				ResetUI();
			}
		} else {
			GameObject ui = uiList[uiList.Count - 1];
			RemoveUI(ui);
		}
	}

	public GameObject CurUI() {
		return uiList.Count > 0 ? uiList[uiList.Count - 1] : null;
	}

	public void RemoveUI(GameObject ui) {
		uiList.Remove (ui);
		Destroy (ui);
		GameObject curUI = CurUI();
		if (curUI != null) {
			curUI.SetActive(true);


//			UIFocusListener uiFocusListener = GetMyComponent<UIFocusListener>(curUI) as UIFocusListener;
			UIFocusListener uiFocusListener = curUI.GetComponent("UIFocusListener") as UIFocusListener;
			if(uiFocusListener != null) {
				uiFocusListener.OnFocus();
			}
		}
	}

	public void ResetUI() {
		if (uiList.Count == 1 && uiList[0].name == defaultWin) {
			return;
		}
		for (int i = 0; i < uiList.Count; i++) {
			GameObject ui = uiList[i];
			Destroy (ui);
		}
		uiList.Clear ();
		LoadUI (defaultWin, null);
	}

	public Component GetMyComponent<T>(GameObject o) {
		Component[] comps = GetComponents<Component> ();
		for (int i = 0; i < comps.Length; i++) {
			Component c = comps[i];
			if(c is T) {
				return c;
			}
		}
		return null;
	}

	public GameObject GetUI(string name) {
		for (int i = 0; i < uiList.Count; i++) {
			GameObject ui = uiList[i];
			if(ui.name == name) {
				return ui;
			}
		}
		return null;
	}

	public GameObject LoadUI(string name, object arg) {
		GameObject curUI = CurUI();
		if (curUI != null) {
//			UIUnFocusListener uiUnFocusListener = GetMyComponent<UIUnFocusListener>(curUI) as UIUnFocusListener;
			UIUnFocusListener uiUnFocusListener = curUI.GetComponent("UIUnFocusListener") as UIUnFocusListener;
			if(uiUnFocusListener != null) {
				uiUnFocusListener.OnUnFocus();
			}
            Destroy(curUI);
			//curUI.SetActive(false);
		}
		Object obj = Resources.Load (name);
		GameObject win = (GameObject)Instantiate (obj);
		win.name = name;
        UIManager.currentWin = name;


        RectTransform rt = win.GetComponent<RectTransform>();
		rt.SetParent (mainCanvas.transform);
		rt.localPosition = Vector3.zero;
		rt.localScale = Vector3.one;
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;
		uiList.Add (win);
		return win;
	}

	public GameObject PopUp(GameObject parant, string name) {
		Object obj = Resources.Load (name);
		GameObject win = (GameObject)Instantiate (obj);
		win.name = name;
		
		RectTransform rt = win.GetComponent<RectTransform>();
		rt.SetParent (parant.transform);
		rt.localPosition = Vector3.zero;
		rt.localScale = Vector3.one;
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;
		return win;
	}

	public void PlayAudio(string name) {
		AudioClip ac = Query<AudioClip> (name);
		if (ac != null && ac.isReadyToPlay) {
			//AudioSource.PlayClipAtPoint(ac, transform.localPosition); 
			music.PlayOneShot(ac);
		}
	}


}
