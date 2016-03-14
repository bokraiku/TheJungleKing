using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SimpleJson;

using System.Security.Cryptography;



public class PlayController : Scope {
    private int count_nopoint = 0;
    private int max_nopoint = 5;
	private JsonArray list;
	public GameObject gx;
	private string no;
	private int count;
	private int ret;
	private object addScore;
	private GameObject winLock;
    private string[] cs = { "pai2", "pai3" };
    public static string is_back;


    void Start() {
        JsonArray bt_status = RootScope.instance.Query<JsonArray>("bt_status");
        string os = SystemInfo.operatingSystem.ToString();
        Regex ios_pattern = new Regex(@"(iphone|ipod|ipad)", RegexOptions.IgnoreCase);





        // Get Rewart Button
        GameObject YR = GameObject.Find("YourReward");

        for (int i = 0; i < bt_status.Count; i++)
        {
            JsonObject obj = (JsonObject)bt_status[i];


            //Debug.Log("IOS : " + UnityEngine.iOS.Device.generation.ToString());
            if (obj["button_name"].ToString().Equals("reward_bt") && obj["active"].ToString().Equals("0") && ios_pattern.Match(os).Success)
            {
                YR.SetActive(false);
            }
        }



        CallBack<JsonObject> Succ = delegate(JsonObject d) {
			if(d["playTimes"].ToString() == "1") {
				GameObject popupWin = UIManager.instance.PopUp (gameObject, "WinPopupxxx");
				Scope popup = popupWin.GetComponent<Scope> ();
				popup.Set ("title", LangUtils.Get("play008"));
				popup.Set ("content", LangUtils.Get("play009"));
			}
		};
		
		JsonObject data = new JsonObject ();
        MD5 md5Hash = MD5.Create();
        string str_checksum = "LoginTarzan|GetPlayTimes|Winner";
        string checksum = RewardUrl.GetMd5Hash(md5Hash, str_checksum);
        data["method"] = checksum;
        M_Data.instance.Request("get_playtime", data, Succ, null);

		MyInfo myInfo = RootScope.instance.Query<MyInfo> ("myInfo");
		if (myInfo != null) {
			Set("myInfo", myInfo);
		}
		NewList ();
		Apply ();
	}

	public void OnClose() {
		if(count == 1) {
			return;
		}
        //PlayController.is_back = "ok";
        PlayerPrefs.SetString("is_back", "ok");
        PlayerPrefs.SetString("is_play", "ok");
        UIManager.instance.LoadUI ("WinRank",null);
        
	}

	public void NewList() {
		count = 0;
		list = new JsonArray ();
		list.Add (new Card ("1", false));
		list.Add (new Card ("2", false));
		list.Add (new Card ("3", false));
		list.Add (new Card ("4", false));
		list.Add (new Card ("5", false));
		list.Add (new Card ("6", false));
		list.Add (new Card ("7", false));
		list.Add (new Card ("8", false));
		list.Add (new Card ("9", false));
		Set ("Card", list);
	}

	public void ResetList() {
		MyInfo info = Query<MyInfo> ("myInfo");
		if (info.score <= 0) {
            
            PlayerPrefs.SetString("is_back", "ok");
            PlayerPrefs.SetString("is_play", "ok");
            UIManager.instance.LoadUI("WinRank", null);
            //UIManager.instance.GoBack();
			return;
		}
		no = null;
		NewList ();
//		useL.Clear ();
//		for (int i = 0; i < list.Count; i++) {
//			Card card = (Card)list[i];
//			card.swipe = false;
//		}
		Apply();
	}
	public void OnCardClick(string ID) {
		Card o = GetCard(ID);
        
		if (o == null) {
			return;
		}
		if (count >= 2) {

			ResetList();
			return;
		}
		if (o.swipe) {
			return;
		}
		if (winLock != null) {
			return;
		}


		winLock = UIManager.instance.PopUp (gameObject, "WinBackDrop");

		CallBack<JsonObject> Succ = delegate(JsonObject d) {

			Destroy(winLock);
			ret = 0;
			count++;
			List<Card> cards = SimpleJson.SimpleJson.DeserializeObject<List<Card>>(d["cards"].ToString());

            

 
			bool tarzanSucc = false;

            Debug.Log("Card Count : " + cards.Count.ToString());

            
            for (int i = 0; i < cards.Count; i++) {
				Card card = cards[i];
				Card c = GetCard(card.ID);
				c.card = card.card;
                
                c.swipe = true;
				if("pai1".Equals(card.card)) {
					tarzanSucc = true;
				}
			}
			if(tarzanSucc) {
				StartCoroutine(SwipeAll(0.5f));
			}
			no = d["no"].ToString();
			object myInfo;
			if(d.TryGetValue("myInfo", out myInfo)) {
				MyInfo mi = SimpleJson.SimpleJson.DeserializeObject<MyInfo> (myInfo.ToString());
				Set("myInfo", mi);
				RootScope.instance.Set("myInfo", mi);
			}

			if(d.ContainsKey("ret")) {
				ret = int.Parse(d["ret"].ToString());
			}
			d.TryGetValue("addScore", out addScore);
			Apply();
			winLock = UIManager.instance.PopUp (gameObject, "WinLock");
			StartCoroutine(OnSwipeFinish(0.5f));

		};

		CallBack<JsonObject> Fail = delegate (JsonObject d) {
			Destroy(winLock);
			GameObject popupWin = UIManager.instance.PopUp (gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope> ();
			popup.Set ("title", LangUtils.Get("play001"));
			popup.Set ("content", d["msg"]);
            //StartCoroutine(BackToRank(1.2f));
            this.count_nopoint++;
            if (this.count_nopoint >= this.max_nopoint )
            {
                StartCoroutine(BackToRank(1.0f));
            }

        };

		JsonObject data = new JsonObject ();
        MD5 md5Hash = MD5.Create();
        //System.Random rnd = new System.Random();

		data ["no"] = no==null?"no":no;
		data ["ID"] = ID;
        data ["nonce"] = System.DateTime.Now.ToString("HH:mm:ss.ffffff") + Random.Range(0, 9999999);
        string str_checksum = "LoginTarzan|" + data["no"] + "|" + data["ID"] + "|" + data["nonce"] + "|Winner";
        string checksum = RewardUrl.GetMd5Hash(md5Hash, str_checksum);
        data["chksum"] = checksum;



        M_Data.instance.Request("suffle_card", data, Succ, Fail);

	}

	public IEnumerator SwipeAll(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		List<string> l = new List<string>();
		l.Add (cs [0]);l.Add (cs [0]);l.Add (cs [0]);l.Add (cs [0]);
		l.Add (cs [1]);l.Add (cs [1]);l.Add (cs [1]);l.Add (cs [1]);
		
		for (int i = 0; i < list.Count; i++) {
			Card card = (Card)list [i];
			if(card.swipe) {
				l.Remove(card.card);
			}
		}
		for(int i = 0; i < list.Count; i++) {
			Card card = (Card)list[i];
			if(!card.swipe) {
				int index = Random.Range(0, l.Count);
				card.card = l[index];
				card.swipe = true;
				l.RemoveAt(index);
			}
		}
		Apply ();
	}


	public Card GetCard(string ID) {
		for (int i = 0; i < list.Count; i++) {
			Card card = (Card)list[i];
			if(card.ID == ID) {
				return card;
			}
		}
		return null;
	}

	public IEnumerator OnSwipeFinish(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		if (winLock != null) {
			UIManager.instance.RemoveUI (winLock);
			winLock = null;
		}
		if (ret > 0) {
    
			GameObject gxObj = (GameObject)Instantiate(gx);
			gxObj.transform.position += new Vector3(0f, 0f, -10f);
			Destroy(gxObj, 1.5f);

			UIManager.instance.PlayAudio ("win");
            StartCoroutine(PrintFlower(0f));
			//StartCoroutine(PrintFlower(0.2f));
            /*
			StartCoroutine(PrintFlower(0.4f));
			StartCoroutine(PrintFlower(0.6f));
			StartCoroutine(PrintFlower(0.8f));
             */
            StartCoroutine(PrintMeteo(1.2f));
			//StartCoroutine(PrintFlower(1.4f));

            /*
			StartCoroutine(PrintFlower(1.6f));
			StartCoroutine(PrintFlower(1.8f));
			StartCoroutine(PrintFlower(2.0f));
			StartCoroutine(PrintFlower(2.2f));
			StartCoroutine(PrintFlower(2.4f));
			StartCoroutine(PrintFlower(2.6f));
			StartCoroutine(PrintFlower(2.8f));
             */

			StartCoroutine(PrintWin(0.8f));
			Wait(0.8f);

			//UIManager.instance.PopUp(gameObject, "xx");
		} else if (ret < 0) {
			GameObject popup = UIManager.instance.PopUp(gameObject, "WinPopupAA");
			//popup.transform.localScale = Vector3.zero;
			PopupController popupController = popup.GetComponent<PopupController>();
			popupController.Set("title", LangUtils.Get("play006"));
			popupController.Set("content", LangUtils.Get(ret == -1 ? "play007" : "play011"));
			popupController.Set("sec", 2.0f);
			popupController.callback = ResetList;
			UIManager.instance.PlayAudio ("shibai");
		}
	}

    IEnumerator PrintMeteo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

          GameObject xx = UIManager.instance.PopUp(gameObject, "LightningStrike");
          xx.transform.localPosition = new Vector3(0, -250, -5);

    }

    IEnumerator PrintBoom(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject xx = UIManager.instance.PopUp(gameObject, "Boom");
        xx.transform.Rotate(14, 0, 0);
        xx.transform.localPosition = new Vector3(0, -20, -5);

    }

	IEnumerator PrintFlower(float waitTime)  
	{  
		yield return new WaitForSeconds(waitTime);  
		for(int i = 0; i < 4; i++) {
			GameObject xx = UIManager.instance.PopUp(gameObject, "xx");
			xx.transform.localPosition = new Vector3(Random.Range(-400, 400), Random.Range(-300, 300), 0);
		}
	}

    IEnumerator BackToRank(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //PlayController.is_back = "ok";
        PlayerPrefs.SetString("is_back", "ok");
        PlayerPrefs.SetString("is_play", "ok");
        UIManager.instance.LoadUI("WinRank", null);
    }

    IEnumerator PrintWin(float waitTime)  
	{
        
		yield return new WaitForSeconds(waitTime);
        string text;
        if (ret == 1)
        {
            text = LangUtils.Get("play010", addScore.ToString());
        }
        else
        {
            text = LangUtils.Get("play005");
        }
        //string text = LangUtils.Get(ret == 1 ? "play005" : "play010", addScore.ToString());
        
		GameObject popup = UIManager.instance.PopUp(gameObject, "WinPopupAA");
		//popup.transform.localScale = Vector3.zero;
		PopupController popupController = popup.GetComponent<PopupController>();
		popupController.Set("title", LangUtils.Get("play004"));
        popupController.Set("content", text);
		popupController.Set("sec", 2.0f);
		popupController.callback = ResetList;
	}
	void Wait(float waitTime)  
	{
		GameObject popup = UIManager.instance.PopUp(gameObject, "WinLock");
		Destroy (popup, waitTime);
	}
}
