using UnityEngine;
using System.Collections;
using SimpleJson;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using SocketIO;

using System.Collections.Generic;
using System.Security.Cryptography;

public class RankController : Scope, UIFocusListener {
	

	private int[] cd = new int[2];
	private bool[] loading = new bool[2];
	public int loadNum = 10;
    //public SocketIOComponent socketIO;
    //public GameObject go;
    private SocketIOComponent socket;
    void Start() {

        //GameObject WinObj = GameObject.Find("WinLogin");
       // Destroy(WinObj);

        JsonArray bt_status = RootScope.instance.Query<JsonArray>("bt_status");
        string os =  SystemInfo.operatingSystem.ToString();
        Regex ios_pattern = new Regex(@"(iphone|ipod|ipad)", RegexOptions.IgnoreCase);

       
        

        
        // Get Rewart Button
        GameObject YR = GameObject.Find("YourReward");

        for (int i = 0;i < bt_status.Count; i++)
        {
            JsonObject obj = (JsonObject)bt_status[i];
            
            
            //Debug.Log("IOS : " + UnityEngine.iOS.Device.generation.ToString());
            if(obj["button_name"].ToString().Equals("reward_bt") && obj["active"].ToString().Equals("0") && ios_pattern.Match(os).Success)
            {
                YR.SetActive(false);
            }
        }
        
        

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        //socket.On("BOOP", OnBOOP);

        if (!PlayerPrefs.GetString("is_play").Equals("ok") && (PlayController.is_back == null || PlayController.is_back != "ok"))
        {
            StartCoroutine("ConnectSocket");
        }
        else
        {
            PlayerPrefs.SetString("is_play", "");
            PlayController.is_back = null;
        }
       

        if (PlayerPrefs.GetString("is_login_repeat").Equals("yes"))
        {
            Debug.Log("Login Repeat");
            StartCoroutine("LoginRepeat");
        }
        

        Set ("type", 0);
		Load (0);

 
	}
    public void OnBOOP(SocketIOEvent e)
    {
        Debug.Log("You are connected: ID : " + e.data.GetField("id"));
    }


    void Update() {
		for (int i = 0; i < cd.Length; i++) {
			if(cd[i] > 0) {
				cd[i]--;
			}
		}
       
	}
    

	public void Load(int type) {
		if (loading [type]) {
			return;
		}
		if (cd [type] > 0) {
			return;
		}


        
		loading [type] = true;
		CallBack<JsonObject> Succ = delegate(JsonObject d) {
			cd [type] = 300;
			loading [type] = false;
            //Debug.Log("MyINFO:" + d["myInfo"].ToString());
            //Debug.Log("LIST : " + SimpleJson.SimpleJson.DeserializeObject<JsonArray>(d["list"].ToString()));
			//MyInfo myInfo = SimpleJson.SimpleJson.DeserializeObject<MyInfo>(d["myInfo"].ToString());

            MyInfo myInfo = SimpleJson.SimpleJson.DeserializeObject<MyInfo>(d["myInfo"].ToString());
            
			JsonArray list = SimpleJson.SimpleJson.DeserializeObject<JsonArray>(d["list"].ToString());

            Debug.Log("MyINFO : " + d["myInfo"].ToString());
			Set("Rank_" + type, list);
			Set("myInfo", myInfo);

            if (list.Count > 0)
            {

                GetComponent<Animator>().enabled = true;
            }
			Apply();
			RootScope.instance.Set("myInfo", myInfo);
		};
		
		CallBack<JsonObject> Fail = delegate (JsonObject d) {
			loading [type] = false;
		};
		
		JsonObject data = new JsonObject ();
        MD5 md5Hash = MD5.Create();

        data ["start"] = 0;
		data ["num"] = loadNum;
        data["nonce"] = System.DateTime.Now.ToString("HH:mm:ss.ffffff") + Random.Range(0, 9999999);
        string str_checksum = "LoginTarzan|" + data["start"] + "|" + data["num"] + "|" + data["nonce"] + "|Winner";
        string checksum = RewardUrl.GetMd5Hash(md5Hash, str_checksum);
        data["chksum"] = checksum;
        M_Data.instance.Request("get_rankinfo", data, Succ, Fail);

   
	}
    private IEnumerator ConnectSocket()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Send Event Play to Server");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = PlayerPrefs.GetString("name");
        data["socket_id"] = socket.sid;
        socket.Emit("LOGIN",new JSONObject(data));

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

    public void OnFocus () {
		MyInfo myInfo = RootScope.instance.Query<MyInfo> ("myInfo");
		if (myInfo != null) {
			Set("myInfo", myInfo);
		}
		for (int i = 0; i < cd.Length; i++) {
			if(cd[i] > 0) {
				cd[i] = 0;
			}
		}
		for (int i = 0; i < loading.Length; i++) {
			if(loading[i]) {
				loading[i] = false;
			}
		}
		Set ("type", 0);
		Load (0);
		Apply ();
	}


	public void OnPlayClick () {
		JsonObject top = RootScope.instance.Query<JsonObject> ("top");
        Debug.Log("TOP : " + top);
        /*
		if (top != null) {
			int t = int.Parse(top["top"].ToString());
			if(t >= 1 && 1 <= 3) {
				GameObject popupWin = UIManager.instance.PopUp (gameObject, "WinPopup");
				Scope popup = popupWin.GetComponent<Scope> ();
				popup.Set ("title", LangUtils.Get("rank003"));
				popup.Set ("content", LangUtils.Get("rank004", top["top"].ToString(), top["score"].ToString()));
			} else {
				GameObject popupWin = UIManager.instance.PopUp (gameObject, "WinPopup");
				Scope popup = popupWin.GetComponent<Scope> ();
				popup.Set ("title", LangUtils.Get("rank005"));
				popup.Set ("content", LangUtils.Get("rank006"));
			}
			return;
		}
         * */
        
        UIManager.instance.PlayAudio ("10");
		UIManager.instance.LoadUI ("WinPlay", null);
	}

	public void OnTabChange(Tab tab) {
		int type = Query<int> ("type");
		Load (type);
	}

	public void LoadMore(ScrollController ctl) {
		int type = 0;
		loading [type] = true;
		CallBack<JsonObject> Succ = delegate(JsonObject d) {
			loading [type] = false;
			ctl.SetLoading(false);
			MyInfo myInfo = SimpleJson.SimpleJson.DeserializeObject<MyInfo>(d["myInfo"].ToString());
			JsonArray list = SimpleJson.SimpleJson.DeserializeObject<JsonArray>(d["list"].ToString());

			JsonArray l = Query<JsonArray>("Rank_" + type);
			if(l == null) {
				Set("Rank_" + type, list);
			} else {
				l.AddRange(list);
			}
			Set("myInfo", myInfo);
			Apply ();
			RootScope.instance.Set("myInfo", myInfo);
		};
		
		CallBack<JsonObject> Fail = delegate (JsonObject d) {
			loading [type] = false;
			ctl.SetLoading(false);
		};

		JsonArray ll = Query<JsonArray>("Rank_" + type);
		JsonObject data = new JsonObject ();
        MD5 md5Hash = MD5.Create();

        data ["start"] = ll == null ? 0 : ll.Count;
		data ["num"] = loadNum;
        data["nonce"] = System.DateTime.Now.ToString("HH:mm:ss.ffffff") + Random.Range(0, 9999999);
        string str_checksum = "LoginTarzan|" + data["start"] + "|" + data["num"] + "|" + data["nonce"] + "|Winner";
        string checksum = RewardUrl.GetMd5Hash(md5Hash, str_checksum);
        data["chksum"] = checksum;
        M_Data.instance.Request("get_rankinfo", data, Succ, Fail);
	}

}
