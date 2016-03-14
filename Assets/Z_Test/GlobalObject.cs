using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalObject : MonoBehaviour
{

    private static GameObject go;

    static GlobalObject()
    {
		if (go == null) {
			go = new GameObject("GlobalObject");
			DontDestroyOnLoad(go);
		}
    }
	
    public static T Create<T>() where T : MonoBehaviour
    {
        return go.AddComponent<T>();
    }
}
