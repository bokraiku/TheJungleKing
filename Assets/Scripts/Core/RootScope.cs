using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class RootScope : Scope {

	public static RootScope instance;

    private SocketIOComponent socket;

    public List<Sprite> cards;
    private bool _PAUSE_STATUS;

	void Awake () {
		instance = this;
		foreach(Sprite card in cards) {
			Set(card.name, card);
		}
	}
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

            if (GameObject.FindGameObjectsWithTag("Popup").Length < 1)
            {

                GameObject popup = UIManager.instance.PopUp(gameObject, "WinPopupOK");
                //popup.transform.localScale = Vector3.zero;
                PopupController popupController = popup.GetComponent<PopupController>();
                popupController.Set("title", "Message");
                popupController.Set("content", "Do you want to exit?");

                //popupController.callback = ExisApp;
            }

            /*
            if (UIManager.currentWin.Equals("WinRank"))
            {
                StartCoroutine("Disconnected");

                //UIManager.instance.LoadUI("WinLogin", null);
            }
            else if (UIManager.currentWin.Equals("WinPlay"))
            {
                UIManager.instance.LoadUI("WinRank", null);

            }
            else
            {
                Application.Quit();
                //StartCoroutine("Disconnected");
            }
            */
          
        }
       
    }
    private void ExisApp()
    {
      
    }

    private IEnumerator Disconnected()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("User is Logout");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = PlayerPrefs.GetString("name");
        data["socket_id"] = socket.sid;
        socket.Emit("disconnect", new JSONObject(data));
        Debug.Log("Current UI : " + UIManager.currentWin);
        PlayerPrefs.SetString("name", "");
        PlayerPrefs.SetString("user", "");
        UIManager.instance.LoadUI("WinLogin", null);

    }

    void OnApplicationPause(bool pauseStatus)
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        if (pauseStatus)
        {
            if (!UIManager.currentWin.Equals("WinLogin")) {
                Debug.Log("User is Logout");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["user"] = PlayerPrefs.GetString("name");
                data["socket_id"] = socket.sid;
                socket.Emit("disconnect", new JSONObject(data));

            }


            if(GameObject.FindGameObjectsWithTag("Popup").Length < 1)
            {

                GameObject popup = UIManager.instance.PopUp(gameObject, "WinPopup");
                //popup.transform.localScale = Vector3.zero;
                PopupController popupController = popup.GetComponent<PopupController>();
                popupController.Set("title", "Message");
                popupController.Set("content", "Game Pause");

                popupController.callback = ResumeApplication;
            }
            
            

            /*
            GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
            Scope popup = popupWin.GetComponent<Scope>();
            popup.Set("title", "Message");
            popup.Set("content", "Game Pause");
            popup.callback = ResumeApplication;
            */


        }
        else
        {

        }

    }
    public void ResumeApplication()
    {
        string username = PlayerPrefs.GetString("name");
        if(username != "" || username.Length >= 32)
        {
            UIManager.instance.LoadUI("WinRank", null);
        }
    }
}
