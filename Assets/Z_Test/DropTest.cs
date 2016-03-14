using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropTest : MonoBehaviour, IDragHandler {



	// Use this for initialization
	void Start () {
	
	}
	static public T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();
		
		if (comp != null)
			return comp;
		
		Transform t = go.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}
	public void OnDrag(PointerEventData data)
	{
//		if (data.pointerEnter == null) {
//			return;
//		}
//		RectTransform m_DraggingPlane = data.pointerEnter.transform as RectTransform;
		//Debug.Log ("===========" + data.position);

		Canvas canvas = FindInParents<Canvas>(gameObject);
		RectTransform m_DraggingPlane = canvas.GetComponent<RectTransform> ();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			RectTransform rt = GetComponent<RectTransform>();
			rt.position = globalMousePos;
		}
	}
}
