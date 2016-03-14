using UnityEngine;
using System.Collections;
using System;
using SimpleJson;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[Serializable]
public class Scope : MonoBehaviour {

	JsonObject scope = new JsonObject();

	[Serializable]
	public class ScopeChangeEvent : UnityEvent {}
	
	// Event delegates triggered on click.
	[FormerlySerializedAs("onScopeChange")]
	[SerializeField]
	private ScopeChangeEvent m_OnScopeChange = new ScopeChangeEvent();
	
	public ScopeChangeEvent onScopeChange {
		get { return m_OnScopeChange; }
		set { m_OnScopeChange = value; }
	}

	public T Query<T>(string key) {
		object value;
		if (!scope.TryGetValue (key, out value)) {
			return default(T);
		}
		if (value is T) {
			return (T)value;
		}
		return SimpleJson.SimpleJson.DeserializeObject<T>(value.ToString());
	}

	public void Set(string key, object value) {
		scope.Remove (key);
		if (value != null) {
			scope.Add(key, value);
		}
	}

	public void Apply() {
		m_OnScopeChange.Invoke ();
	}
}
