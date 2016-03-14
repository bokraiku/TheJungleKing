using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LangUtils : MonoBehaviour {

	public string language;
	public static LangUtils instance;
	private Dictionary<string, string> dic = new Dictionary<string, string>();
	
	void Awake () {
		instance = this;
		TextAsset txt = Resources.Load(language, typeof(TextAsset)) as TextAsset;
		if (txt != null) {
			Load(txt);
		}
		Debug.Log(Get ("test", "1000", 123));
	}

	void Load(TextAsset txt) {
		//Debug.Log (txt.ToString ());
		char[] tc = {':','='};
		char[] sc = {'\n'};
		string s = txt.ToString ();
		string[] st = s.Split (sc, System.StringSplitOptions.RemoveEmptyEntries);
		for(int i = 0; i < st.Length; i++) {
			st[i] = st[i].Trim();
			if(st[i] == "" || st[i].StartsWith("#")) {
				continue;
			}
			int index = st[i].IndexOfAny(tc);
			if(index == -1) {
				continue;
			}
			string k = st[i].Substring(0, index);
			string v = st[i].Substring(index + 1, st[i].Length - index - 1);
			dic[k] = v;
		}
	}

	public static string Get(string key, params object[] os) {
		string v;
		if (!instance.dic.TryGetValue(key, out v)) {
			return null;
		}
		return string.Format(v, os);
	}

}
