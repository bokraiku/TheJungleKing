using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Touch : MonoBehaviour, IPointerClickHandler {

	[Serializable]
	public class TouchEvent : UnityEvent {}
	
	// Event delegates triggered on click.
	[FormerlySerializedAs("onTouch")]
	[SerializeField]
	private TouchEvent m_OnTouch = new TouchEvent();
	
	public TouchEvent onTouch {
		get { return m_OnTouch; }
		set { m_OnTouch = value; }
	}


	public void OnPointerClick(PointerEventData eventData) {
		m_OnTouch.Invoke ();
	}
}
