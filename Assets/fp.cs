using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class fp : MonoBehaviour {

	[Serializable]
	public class FpClickedEvent : UnityEvent {}
	
	// Event delegates triggered on click.
	[FormerlySerializedAs("onCenter")]
	[SerializeField]
	private FpClickedEvent m_OnCenter = new FpClickedEvent();
	
	public FpClickedEvent onCenter
	{
		get { return m_OnCenter; }
		set { m_OnCenter = value; }
	}


	// Event delegates triggered on click.
	[FormerlySerializedAs("onEnd")]
	[SerializeField]
	private FpClickedEvent m_OnEnd = new FpClickedEvent();
	
	public FpClickedEvent onEnd
	{
		get { return m_OnEnd; }
		set { m_OnEnd = value; }
	}

	public void PlayEnd () {
		m_OnEnd.Invoke ();
		Destroy (gameObject);
	}

	public void PlayCenter() {
		m_OnCenter.Invoke ();
	}

}
