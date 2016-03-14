using UnityEngine;
using System.Collections;
using SimpleJson;
using UnityEngine.UI;

public class ItemRank : Scope {
	public Text nameText;
	public Text integralText;
	public Image topImage;
	public Text topText;
	private string[] imgs = {"phbn", "phb1", "phb2", "phb3"};


	// Use this for initialization
	void Start () {
        
		onScopeChange.AddListener (OnScopeChange);
		OnScopeChange ();
	}

	public void OnScopeChange() {

        
		TopInfo info = Query<TopInfo> (name);
		if (info == null) {
			return;
		}
		nameText.text = info.name;
		integralText.text = info.score.ToString();
		if (info.top > 3) {
			topImage.sprite = RootScope.instance.Query<Sprite>(imgs[0]);
		} else {
			topImage.sprite = RootScope.instance.Query<Sprite>(imgs[info.top]);
		}
		topText.text = info.top.ToString ();
	}
}
