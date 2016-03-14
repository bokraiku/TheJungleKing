using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJson;
using System.Text.RegularExpressions;

public class AccountController : Scope {

	public InputField password;
	public InputField passwordConfirm;
	public InputField phone;
	public InputField phoneConfirm;

	// Use this for initialization
	void Start () {
		string name = PlayerPrefs.GetString ("name");
		if (name != null) {
			Set ("name", name);
		}
	}


	public void OnGoClick() {
		UIManager.instance.PlayAudio ("10");
		string pa = password.text.Trim ();
		if (pa == "") {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("account001"));
			popup.Set("content", LangUtils.Get("account002"));
			return;
		}
		string pacf = passwordConfirm.text.Trim ();
		if (pa != pacf) {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("account001"));
			popup.Set("content", LangUtils.Get("account003"));
			return;
		}

		string ph = phone.text.Trim ();
		if (ph == "") {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("account001"));
			popup.Set("content", LangUtils.Get("account004"));
			return;
		}
		//const string pattern = @"^1[3578]\d{9}";
		const string pattern = @"^\d{5,}";
		if (!Regex.IsMatch (ph, pattern)) {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("account001"));
			popup.Set("content", LangUtils.Get("account005"));
			return;
		}

		string phcf = phoneConfirm.text.Trim ();
		if (ph != phcf) {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("account001"));
			popup.Set("content", LangUtils.Get("account006"));
			return;
		}

		//NSString *phoneRegex = @"1[3|5|7|8|][0-9]{9}";



		GameObject backDrop = UIManager.instance.PopUp (gameObject, "WinBackDrop");

		CallBack<JsonObject> Succ = delegate (JsonObject d) {
			UIManager.instance.RemoveUI (gameObject);
			UIManager.instance.LoadUI ("WinRank", null);
		};
		
		CallBack<JsonObject> Fail = delegate (JsonObject d) {
			GameObject popupWin = UIManager.instance.PopUp(gameObject, "WinPopup");
			Scope popup = popupWin.GetComponent<Scope>();
			popup.Set("title", LangUtils.Get("account007"));
			popup.Set("content", d["msg"]);
			Destroy(backDrop);
		};

		JsonObject data = new JsonObject ();
		data ["password"] = pa;
		data ["phone"] = ph;
		M_Data.instance.Request("0102", data, Succ, Fail);
	}

}
