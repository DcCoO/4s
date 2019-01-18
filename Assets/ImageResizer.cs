using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageResizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float n = gameObject.rectTransform().localScale.x;
        float s = (n * ((float)Screen.width)) / 600f;
        gameObject.rectTransform().localScale = s * Vector2.one;
    }
	
}

//n - 600
//s - w