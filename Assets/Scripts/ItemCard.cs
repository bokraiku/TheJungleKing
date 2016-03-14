using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ItemCard : Scope, IPointerClickHandler {

	private string ID;
	private bool swipe;
	public Image back;
	private bool shuffle;
	public GameObject b;
	private Vector3 p;

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
       
        
		onScopeChange.AddListener (OnScopeChange);
		OnScopeChange ();
	}


	void Update() {
		if (shuffle) {
			ItemCard[] cards = transform.parent.GetComponentsInChildren<ItemCard> ();
			Transform front = transform.GetChild (0);
			front.localPosition = cards [4].transform.localPosition - transform.localPosition;
			shuffle = false;
		} else {
			Transform front = transform.GetChild (0);
			if (front.localPosition != Vector3.zero) {
				front.localPosition = Vector3.Lerp(front.localPosition, Vector3.zero, Time.deltaTime * 7);
				//front.localPosition = Vector3.MoveTowards(front.localPosition, Vector3.zero, Time.deltaTime * 300);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
//		Animator animator = GetComponent<Animator> ();
//		animator.SetBool ("swipe", true);
		p = transform.position;
		//Debug.Log(transform.gameObject.GetHashCode() + "----" + transform.position);
		m_OnItemClick.Invoke (ID);
	}

	void PlayFront() {
		Animator animator = GetComponent<Animator> ();
		if (swipe && !animator.GetBool("swipe")) {
			animator.SetBool ("swipe", swipe);
		}
	}
	

	public void OnScopeChange() {
        
		Card o = Query<Card> (name);
		if (o == null) {
			return;
		}
        object test;
        if (o.card == null || o.card == "")
        {
            test = null;
        }
        else
        {
            test = RootScope.instance.Query<Sprite>(o.card);
        }
        //object test = o.card == null ? null : RootScope.instance.Query<Sprite>(o.card);

        back.sprite = o.card == null ? null : RootScope.instance.Query<Sprite>(o.card);

        //Debug.Log("Back Card : " + o.card);
       
        //Debug.Log("ConD : " + RootScope.instance.Query<Sprite>("pai2"));
		ID = o.ID;
        //Debug.Log("Back Card : " + back.sprite);
        swipe =  o.swipe;
		Animator animator = GetComponent<Animator> ();
		if (swipe && !animator.GetBool("swipe")) {
			//Debug.Log(transform.gameObject.GetHashCode() + "++++" + transform.position);
			UIManager.instance.PlayAudio("fanpai");
			GameObject ff = UIManager.instance.PopUp (b, "fp");
			ff.transform.position = p;
			fp f = ff.GetComponent<fp> ();
			f.onCenter.AddListener (delegate{PlayFront();});
		}

		if (o.shuffle) {
			shuffle = o.shuffle;
			o.shuffle = false;
		}
	}
}
