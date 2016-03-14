using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using SocketIO;

public class PopupController : Scope {
	public Text title;
	public Text content;
	public Button ok;
    public Button no;
    public Button exit;
	public UnityAction callback;
    private SocketIOComponent socket;

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        if (ok != null) {
			ok.onClick.AddListener (OnOkClick);
		}

        if(exit != null)
        {
            exit.onClick.AddListener(OnOKExitClick);
        }
        if(no != null)
        {
            no.onClick.AddListener(OnNoClick);
        }
		onScopeChange.AddListener (OnScopeChange);
		OnScopeChange ();
	}

	public void OnScopeChange() {
		string t = Query<string> ("title");
		if (title != null) {

            title.text = t == null ? "" : t.Replace("NEWLINE", "\n"); ;
            
		}
		
		string c = Query<string> ("content");
		if (content != null) {

            content.text = c == null ? "" : c.Replace("NEWLINE", "\n"); ;
            
		}

		float sec = Query<float> ("sec");
		if (sec > 0) {
			StartCoroutine(WaitAndDestory(sec));
		}
	}

	void OnOkClick() {
		UIManager.instance.PlayAudio ("10");
		if (callback != null) {
			callback.Invoke ();
		}
		Destroy (gameObject);
	}

    void OnOKExitClick()
    {
        StartCoroutine("Disconnected");
        //Application.Quit();
    }
    private IEnumerator Disconnected()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("User is Logout");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = PlayerPrefs.GetString("name");
        data["socket_id"] = socket.sid;
        socket.Emit("disconnect", new JSONObject(data));
        Debug.Log("Current UI : " + UIManager.currentWin);
        PlayerPrefs.SetString("name", "");
        PlayerPrefs.SetString("user", "");
        Application.Quit();

    }

    void OnNoClick()
    {
        Destroy(gameObject);
    }

	IEnumerator WaitAndDestory(float waitTime)  
	{  
		yield return new WaitForSeconds(waitTime);  
		ClosePupup ();
	}

	public void ClosePupup() {
		if(ok != null) {
			ok.onClick.Invoke();
		}
		Destroy (gameObject);
	}

}
