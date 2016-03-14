using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class TextRoll : MonoBehaviour {
	
	[Serializable]
	public class RollEndEvent : UnityEvent {}
	
	// Event delegates triggered on click.
	[FormerlySerializedAs("onScopeChange")]
	[SerializeField]
	private RollEndEvent m_OnRollEnd = new RollEndEvent();
	
	public RollEndEvent onRollEnd {
		get { return m_OnRollEnd; }
		set { m_OnRollEnd = value; }
	}

	void Update () {
		RectTransform rt = GetComponent<RectTransform> ();
		rt.localPosition -= new Vector3 (Time.deltaTime * 60, 0, 0);
		if (rt.anchoredPosition.x <= -rt.sizeDelta.x) {
			m_OnRollEnd.Invoke();
			Destroy(gameObject);
		}

	}
}
