using UnityEngine;
using System.Collections;
using SimpleJson;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class M_Data : MonoBehaviour {

	public static M_Data instance;
	
	private string COOKIE = null;
	
	void Awake() {
		instance = this;
	}

//	private string loginAction;
//	private JsonObject loginData;
//
//	public void SetLoginInfo(String action, JsonObject data) {
//		loginAction = action;
//		loginData = data;
//	}
//
//	public void reLogin() {
//	}

	public void Request (string action, JsonObject data, CallBack<JsonObject> succ, CallBack<JsonObject> fail)
	{
		CallBack<string> error = delegate(string code) {
			if(code == "n500") {
				UIManager.instance.ResetUI();
				return;
			}
			if (fail != null) {
				JsonObject json = new JsonObject();
				json["msg"] = LangUtils.Get("net001");
				fail(json);
			}
		};
        StartCoroutine(PostData(LangUtils.Get("url_post") + action, data, succ, fail, error));

		//StartCoroutine(requestURL (LangUtils.Get("url") + "/go?cmd=" + action + "&data=" + WWW.EscapeURL(data.ToString()), succ, fail, error));
	}

    IEnumerator PostData(string url, JsonObject PostData, CallBack<JsonObject> succ, CallBack<JsonObject> fail, CallBack<string> error)
    {
        Debug.Log("request:" + url);
        Dictionary<string, string> headers = new Dictionary<string, string>();

       // Hashtable headers = new Hashtable();

        WWWForm form = new WWWForm();


        //Hashtable headers = new Hashtable();
        if (COOKIE != null)
        {
            headers.Add("Content-Type", "application/json");
            headers.Add("Cookie", COOKIE);
            //headers["Cookie"] = COOKIE;
            //headers.Add("Cookie", COOKIE);
        }

        //form.headers = headers;


        foreach (var items in PostData)
        {
            form.AddField(items.Key.ToString(), items.Value.ToString());
        }

        byte[] body = Encoding.UTF8.GetBytes(PostData.ToString());

        byte[] rawData = form.data;
        //form.AddBinaryData("fn", rawData);




        WWW www = new WWW(url, body, headers);
        yield return www;
        if (!String.IsNullOrEmpty(www.error))
        {
            Debug.Log("response:" + www.error + " - ERROR");
            if (error != null)
            {
                error("100");
            }
        }
        else
        {
            
            foreach(var item in www.responseHeaders)
            {
                Debug.Log("response Header |" +item.Key + ":" + item.Value);
            }

            //COOKIE = www.responseHeaders["SET-COOKIE"].Split(';')[0];

            if (COOKIE == null || (COOKIE != www.responseHeaders["SET-COOKIE"].Split(';')[0] && www.responseHeaders["SET-COOKIE"].Split(';')[0] != "FAIL"))
             {
                 COOKIE = www.responseHeaders["SET-COOKIE"].Split(';')[0];
             }



             string state = www.responseHeaders["STATE"];
             Debug.Log("response:" + www.text + " - " + state);

 
             JsonObject json = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(www.text);
             
             if (state == "SUCC")
             {
                 if (succ != null)
                 {
                     //Debug.Log("Test Json:" + json["user"]);
                     succ(json);
                 }
             }
             else if (state == "FAIL")
             {
                 if (fail != null)
                 {
                     fail(json);
                 }
             }
             else if (state == "ERROR")
             {
                 if (error != null)
                 {
                     error(json["code"].ToString());
                 }
             }
             else
             {
                 if (error != null)
                 {
                     error("unknown");
                 }
             } 




        }
            
             
        
        www.Dispose();

    }
	
	IEnumerator requestURL(string url, CallBack<JsonObject> succ, CallBack<JsonObject> fail, CallBack<string> error) {
		Debug.Log("request:" + url);
		Dictionary<string, string> headers = new Dictionary<string, string> ();
		if (COOKIE != null) {
			headers["Cookie"] = COOKIE;
		}
		byte[] bytes = null;
		WWW www = new WWW (url, bytes,headers);
		// 等待数据返回
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			if (COOKIE == null) {
				COOKIE = www.responseHeaders ["SET-COOKIE"].Split (';') [0];
			}
			string state = www.responseHeaders["STATE"];
			Debug.Log ("response:" + www.text + " - " + state);
			JsonObject json = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(www.text);
			if(state == "SUCC") {
				if(succ != null) {
					succ(json);
				}
			} else if(state == "FAIL") {
				if(fail != null) {
					fail(json);
				}
			} else if(state == "ERROR") {
				if(error != null) {
					error(json["code"].ToString());
				}
			} else {
				if(error != null) {
					error("unknown");
				}
			}
		} else {
			Debug.Log ("response:" + www.error + " - ERROR");
			if(error != null) {
				error("100");
			}
		}
       
		www.Dispose();
	}
}
