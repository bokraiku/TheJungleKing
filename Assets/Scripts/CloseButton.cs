using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;
public class CloseButton : Scope {


    private SocketIOComponent socket;
    public void OnCloseClick () {

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        
        //StartCoroutine("Disconnected");

        if (UIManager.currentWin == "WinRank")
        {
            StartCoroutine("Disconnected");
            //UIManager.instance.LoadUI("WinLogin", null);
        }
        else if (UIManager.currentWin == "WinPlay")
        {
            UIManager.instance.LoadUI("WinRank", null);
        }
        else
        {
            StartCoroutine("Disconnected");
        }
  

       
        
	}


    private IEnumerator Disconnected()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("User is Logout");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = PlayerPrefs.GetString("name");
        data["socket_id"] = socket.sid;
        socket.Emit("disconnect", new JSONObject(data));
        Debug.Log("Current UI : "+ UIManager.currentWin);
        PlayerPrefs.SetString("name", "");
        PlayerPrefs.SetString("user", "");
        UIManager.instance.LoadUI("WinLogin", null);

    }

}
