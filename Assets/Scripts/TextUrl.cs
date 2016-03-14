using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextUrl : MonoBehaviour, IPointerClickHandler {
	public string url;

	public void OnPointerClick(PointerEventData eventData) {
		Application.OpenURL (url);
	}
}
