using UnityEngine;
using System.Collections;
using System;
using SimpleJson;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ItemUser : Scope {
	public Text ID;
	public Text countDown;
	public Text integral;
	public Image modify;
	public Sprite m1;
	public Sprite m2;

	[Serializable]
	public class ItemClickEvent : UnityEvent<string> {}
	
	// Event delegates triggered on click.
	[FormerlySerializedAs("onItemClick")]
	[SerializeField]
	private ItemClickEvent m_OnItemClick = new ItemClickEvent();
	
	public ItemClickEvent onItemClick {
		get { return m_OnItemClick; }
		set { m_OnItemClick = value; }
	}


	void Start () {
		Init ();
	}

	void Init() {
		JsonObject i = Query<JsonObject> (name);
		if (i == null) {
			return;
		}
		string id = i ["ID"].ToString ();
		ID.text = id;
		countDown.text = i ["CountDown"].ToString ();
		integral.text = i ["Integral"].ToString ();
		if (i ["Flag"].ToString () == "1") {
			modify.sprite = m1;
			modify.enabled = true;
		} else if (i ["Flag"].ToString () == "2") {
			modify.sprite = m2;
			modify.enabled = true;
		}
		Button b = GetComponent<Button> ();
		b.onClick.AddListener (delegate {
			m_OnItemClick.Invoke(id);
		});
	}
}
