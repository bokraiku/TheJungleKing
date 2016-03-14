using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class ItemRepeat : MonoBehaviour {
	
	public Scope scope;
	public int min;

	private List<object> ol = new List<object> ();
	private List<object> newOL = new List<object> ();
	private List<GameObject> l = new List<GameObject> ();
	private List<GameObject> useL = new List<GameObject>();
	private List<GameObject> newL = new List<GameObject>();
	private int siblingIndex;

	void Awake () {
		siblingIndex = transform.GetSiblingIndex ();
		Scope s = GetComponent<Scope>();
		if (s != null) {
			s.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		OnScopeChange ();
		scope.onScopeChange.AddListener (OnScopeChange);
	}
	
	private GameObject newObj(object d) {
		GameObject o = (GameObject)Instantiate(gameObject);
		o.SetActive(true);
		o.name = name;
		ItemRepeat ir = o.GetComponent<ItemRepeat>();
		ir.enabled = false;
		
		o.transform.SetParent(transform.parent);
		o.transform.localPosition = Vector3.zero;
		o.transform.localScale = Vector3.one;

		Scope s = o.GetComponent<Scope>();
		if(s != null) {
			s.enabled = true;
			s.Set(name, d);
		}
		return o;
	}

	public void OnScopeChange() {
		JsonArray list = scope.Query<JsonArray> (name);
		if (list != null) {
			// 生成新的列表
			for(int i = 0; i < list.Count; i++) {
				object o = list[i];
				newOL.Add(o);
				int idx = ol.IndexOf(o);
				if(idx == -1) {
					newL.Add(newObj(o));
					continue;
				}
				GameObject go = l[idx];
				go.transform.SetParent(null);
				go.transform.SetParent(transform.parent);
				useL.Add(go);
				newL.Add(go);
			}
		}


		// 删除旧的已经不再用的元素
		for(int i = useL.Count - 1; i >= 0; i--) {
			GameObject go = useL[i];
			l.Remove(go);

			Scope s = go.GetComponent<Scope>();
			if(s != null) {
				s.Apply();
			}
			useL.RemoveAt(i);
		}
		for(int i = l.Count - 1; i >= 0; i--) {
			Destroy(l[i]);
			l.RemoveAt(i);
		}

		ol.Clear ();

		// 重新整理列表
		List<GameObject> tL = l;
		l = newL;
		newL = tL;

		List<object> tOL = ol;
		ol = newOL;
		newOL = tOL;

		// 不够min数量的补上
		if(l.Count < min) {
			for(int i = l.Count; i < min; i++) {
				GameObject o = (GameObject)Instantiate(gameObject);
				o.SetActive(true);
				o.name = name;
				
				ItemRepeat ir = o.GetComponent<ItemRepeat>();
				ir.enabled = false;
				o.transform.SetParent(transform.parent);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				l.Add(o);
			}
		}

		// 设置顺序
		for (int i = 0; i < l.Count; i++) {
			GameObject o = l[i];
			o.transform.SetSiblingIndex(siblingIndex + i + 1);
		}
	}
}
