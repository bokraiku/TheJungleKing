using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tab : MonoBehaviour {
	public RectTransform ct;
	public bool sl;


	public void SetSelect(bool select) {
		sl = select;
		ct.gameObject.SetActive(select);
		if (select) {
			ScrollRect rect = ct.parent.GetComponent<ScrollRect> ();
			rect.content = ct;
		}
	}

	void Update() {
		Animator animator = GetComponent<Animator> ();
		if (animator != null) {
			if(sl) {
				animator.Play("TabSL", 0);
			} else {
				animator.Play("TabNotSL", 0);
			}
			//GetComponent<Animator>().SetBool("sl", sl);
		}
	}
}
