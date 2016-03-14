using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ScrollController : MonoBehaviour, IEndDragHandler {

	public Scrollbar bar;
	public GameObject show;
	private bool loading;

	[Serializable]
	public class LoadingEvent : UnityEvent<ScrollController> {}

	[FormerlySerializedAs("onLoading")]
	[SerializeField]
	private LoadingEvent m_OnLoading = new LoadingEvent();
	
	public LoadingEvent onLoading {
		get { return m_OnLoading; }
		set { m_OnLoading = value; }
	}

	void Start() {
		SetLoading(false);
	}
	
	public void OnEndDrag(PointerEventData eventData) {
		if (!loading && bar.value <= 0) {
			SetLoading(true);
			m_OnLoading.Invoke(this);
		}
	}

	public void SetLoading(bool loading) {
		this.loading = loading;
		if(show != null) {
			show.SetActive(loading);
		}
	}
}
