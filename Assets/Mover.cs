using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.rectTransform().localPosition = new Vector2(0, -10000);
	}
}
