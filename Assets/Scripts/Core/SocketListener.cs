using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine.UI;

using SocketIO;

public class SocketListener :  Scope {

    // Use this for initialization
    private SocketIOComponent socket;
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("USER_LOGIN_REPEATED", user_login_repeat);
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    public void user_login_repeat(SocketIOEvent e)
    {
        Debug.Log("Is Black : " + PlayerPrefs.GetString("is_back"));
        if(PlayerPrefs.GetString("is_back") == null || PlayerPrefs.GetString("is_back") != "ok")
        {
            Debug.Log("USER_LOGIN_REPEATED");
            string server_user = PlayerPrefs.GetString("name") + "_" + socket.sid;
            string user_id = JsonToString(e.data.GetField("user_id").ToString(), "\"");

            Debug.Log("Server : " + server_user);
            Debug.Log("Socket : " + user_id);
            if (server_user.Equals(user_id))
            {

                GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
                Scope popup = popupWin.GetComponent<Scope>();
                popup.Set("title", "Game Stop");
                popup.Set("content", "This ID already connect on other device");
                Debug.Log("This ID connected on other device");
                StartCoroutine("CloseApp");

            }
        }
        else
        {
            
            PlayerPrefs.SetString("is_back", "");
        }
        
        

    }
    private IEnumerator CloseApp()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
    string JsonToString(string target, string s)
    {

        string[] newString = Regex.Split(target, s);

        return newString[1];

    }
}
