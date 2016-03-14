using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJson;
using System.Collections.Generic;
using System.Security.Cryptography;
using SocketIO;


public class LoginController : Scope {

	public InputField account;
	public InputField password;
    private SocketIOComponent socket;



    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        string name = PlayerPrefs.GetString ("name");
		if (name != null) {
			account.text = name;
		}

	}

    public void OnGuestClick()
    {
        UIManager.instance.PlayAudio("10");
        string device_id = SystemInfo.deviceUniqueIdentifier;
        string pass = device_id;
        Debug.Log("Device ID : " + device_id);
        LoginGuest(device_id, pass);

    }

    public void OnLoginClick() {
		UIManager.instance.PlayAudio ("10");
		string ac = account.text.Trim ();
		if (ac == "") {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("login001"));
			popup.Set("content", LangUtils.Get("login002"));
			return;
		}
		string pa = password.text.Trim ();
		if (pa == "") {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("login001"));
			popup.Set("content", LangUtils.Get("login003"));
			return;
		}

		Login (ac, pa);
	}

    

    public void Login(string ac, string pa) {
		GameObject backDrop = UIManager.instance.PopUp (gameObject, "WinBackDrop");
		
		CallBack<JsonObject> Succ = delegate (JsonObject d) {
            //Debug.Log("Success Data :" + d);

            
			PlayerPrefs.SetString ("name", ac);
			PlayerPrefs.SetString ("user", d["user"].ToString());
            PlayerPrefs.SetString("is_login_repeat", d["is_login_repeat"].ToString());

            
                UIManager.instance.LoadUI("WinRank", null);

                if (d.ContainsKey("top"))
                {
                    RootScope.instance.Set("top", SimpleJson.SimpleJson.DeserializeObject<JsonObject>(d["top"].ToString()));
                }
                else {
                    RootScope.instance.Set("top", null);
                }

            if (d.ContainsKey("bt_status"))
            {
                RootScope.instance.Set("bt_status", SimpleJson.SimpleJson.DeserializeObject<JsonArray>(d["bt_status"].ToString()));
            }
            else
            {
                RootScope.instance.Set("bt_status", null);
            }

            

            //Debug.Log("BT_STAUS " + SimpleJson.SimpleJson.DeserializeObject<JsonArray>(d["bt_status"].ToString()));
            Destroy(backDrop);



//			JsonObject user = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(d["user"].ToString());
//			int identify = int.Parse(user["identify"].ToString());
//			if(identify < 2) {
//				UIManager.instance.RemoveUI (gameObject);
//				UIManager.instance.LoadUI ("WinAccount", null);
//			} else {
//				UIManager.instance.RemoveUI (gameObject);
//				UIManager.instance.LoadUI ("WinRank", null);
//			}

			
       
		};
		
		CallBack<JsonObject> Fail = delegate (JsonObject d)
        {

            GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
            Scope popup = popupWin.GetComponent<Scope>();
            popup.Set("title", LangUtils.Get("login004"));
            popup.Set("content", d["msg"]);
            Destroy(backDrop);
   
		};
		
		JsonObject data = new JsonObject ();
        MD5 md5Hash = MD5.Create();
        
		data ["appID"] = LangUtils.Get("appID");
		data ["name"] = ac;
		data ["password"] = pa;
        data ["os"] = SystemInfo.operatingSystem.ToString();
        data ["model"] = SystemInfo.deviceModel.ToString();
        data ["imei"] = SystemInfo.deviceUniqueIdentifier.ToString();
        data ["nonce"] = System.DateTime.Now.ToString("HH:mm:ss.ffffff") + Random.Range(0, 9999999);

        string str_checksum = "LoginTarzan|" + data["name"] + "|" + data["password"] + "|"+ data["nonce"]+ "|Winner";
        string checksum = RewardUrl.GetMd5Hash(md5Hash, str_checksum);
        data ["chksum"] = checksum;
       
 
		M_Data.instance.Request("login", data, Succ, Fail);
	}

    public void LoginGuest(string ac, string pa)
    {
        GameObject backDrop = UIManager.instance.PopUp(gameObject, "WinBackDrop");

        CallBack<JsonObject> Succ = delegate (JsonObject d)
        {
            //Debug.Log("Success Data :" + d);


            PlayerPrefs.SetString("name", d["user"].ToString());
            PlayerPrefs.SetString("user", d["user"].ToString());

            //			JsonObject user = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(d["user"].ToString());
            //			int identify = int.Parse(user["identify"].ToString());
            //			if(identify < 2) {
            //				UIManager.instance.RemoveUI (gameObject);
            //				UIManager.instance.LoadUI ("WinAccount", null);
            //			} else {
            //				UIManager.instance.RemoveUI (gameObject);
            //				UIManager.instance.LoadUI ("WinRank", null);
            //			}

            UIManager.instance.LoadUI("WinRank", null);

            if (d.ContainsKey("top"))
            {
                RootScope.instance.Set("top", SimpleJson.SimpleJson.DeserializeObject<JsonObject>(d["top"].ToString()));
            }
            else
            {
                RootScope.instance.Set("top", null);
            }

            if (d.ContainsKey("bt_status"))
            {
                RootScope.instance.Set("bt_status", SimpleJson.SimpleJson.DeserializeObject<JsonArray>(d["bt_status"].ToString()));
            }
            else
            {
                RootScope.instance.Set("bt_status", null);
            }
            Destroy(backDrop);

        };

        CallBack<JsonObject> Fail = delegate (JsonObject d)
        {
            GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
            Scope popup = popupWin.GetComponent<Scope>();
            popup.Set("title", LangUtils.Get("login004"));
            popup.Set("content", d["msg"]);
            Destroy(backDrop);
        };

        JsonObject data = new JsonObject();
        MD5 md5Hash = MD5.Create();

        data["appID"] = LangUtils.Get("appID");
        data["name"] = ac;
        data["password"] = pa;

        data["os"] = SystemInfo.operatingSystem.ToString();
        data["model"] = SystemInfo.deviceModel.ToString();
        data["imei"] = SystemInfo.deviceUniqueIdentifier.ToString();
        data["nonce"] = System.DateTime.Now.ToString("HH:mm:ss.ffffff") + Random.Range(0, 9999999);

        string str_checksum = "LoginTarzan|" + data["name"] + "|" + data["password"] + "|" + data["nonce"] + "|Winner";
        string checksum = RewardUrl.GetMd5Hash(md5Hash, str_checksum);
        data["chksum"] = checksum;


        


        M_Data.instance.Request("login_guest", data, Succ, Fail);
    }

    private IEnumerator LoginRepeat()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("User is Login Repeat");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = PlayerPrefs.GetString("name");
        data["socket_id"] = socket.sid;
        socket.Emit("LOGIN_REPEAT", new JSONObject(data));


    }
    private IEnumerator Disconnected()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("User is Logout");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = PlayerPrefs.GetString("name");
        data["socket_id"] = socket.sid;
        socket.Emit("disconnect", new JSONObject(data));

    }
}
