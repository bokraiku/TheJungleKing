using UnityEngine;
using System.Collections;
using SimpleJson;
using UnityEngine.UI;

public class ItemMyRank : MonoBehaviour {
	public Scope scope;
	public Text nameText;
	public Text integralText;
	public Image topImage;
	public Text topText;
	private string[] imgs = {"phbn", "phb1", "phb2", "phb3"};


	// Use this for initialization
	void Start () {
		scope.onScopeChange.AddListener (OnScopeChange);
		OnScopeChange ();
	}

	public void OnScopeChange() {
		MyInfo info = scope.Query<MyInfo> ("myInfo");
		if (info == null) {
			return;
		}
		nameText.text = info.name;

		int type = scope.Query<int> ("type");
		int topNum = 0;
		int score = 0;
		if (type == 0) {
			score = info.mscore;
			topNum = info.wtop;
		} else {
			score = info.mscore;
			topNum = info.mtop;
		}
		integralText.text = score.ToString ();

		if (topNum > 3) {
			topImage.sprite = RootScope.instance.Query<Sprite>(imgs[0]);
		} else {
			topImage.sprite = RootScope.instance.Query<Sprite>(imgs[topNum]);
		}

		topText.text = topNum.ToString ();
	}
}
