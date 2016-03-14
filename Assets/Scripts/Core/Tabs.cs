using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Tabs : MonoBehaviour {
	public Tab[] tabs;
	private Tab curTab;
	public Scope scope;
	public string bindName;
	private int curIndex;

	[Serializable]
	public class TabClickedEvent : UnityEvent<Tab> {}
	
	// Event delegates triggered on click.
	[FormerlySerializedAs("onClick")]
	[SerializeField]
	private TabClickedEvent m_OnClick = new TabClickedEvent();

	public TabClickedEvent onClick
	{
		get { return m_OnClick; }
		set { m_OnClick = value; }
	}

	// Use this for initialization
	void Start () {
		scope.onScopeChange.AddListener (OnValueChange);
		if (tabs != null && tabs.Length > 0) {
			for(int i = 0; i < tabs.Length; i++) {
				Tab tab = tabs[i];
				Button button = tab.GetComponent<Button>();
				if(button != null) {
					button.onClick.AddListener(delegate {
						this.OnClick(tab); 
					});
				}
			}
			curIndex = -1;
			OnValueChange();
		}
	}

	public void OnValueChange() {
		if (tabs != null && tabs.Length > 0) {
			int index = scope.Query<int> (bindName);
			if(curIndex == index) {
				return;
			}
			for(int i = 0; i < tabs.Length; i++) {
				Tab tab = tabs[i];
				if(i == index) {
					curIndex = index;
					curTab = tab;
					tab.SetSelect (true);
				} else {
					tab.SetSelect (false);
				}
			}
		}
	}

	public void OnClick(Tab tab) {
		if (curTab == tab) {
			return;
		}
		for (int i = 0; i < tabs.Length; i++) {
			if(tabs[i] == tab) {
				curIndex = i;
				break;
			}
		}
		tab.SetSelect (true);
		curTab.SetSelect (false);
		curTab = tab;
		scope.Set (bindName, curIndex);
		m_OnClick.Invoke(curTab);
		scope.Apply ();
	}
}
