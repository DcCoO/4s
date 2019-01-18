using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenPiece : MonoBehaviour {

    public RectTransform rt;
    public Image img;

    private Vector2 dir;
    private float speed;
	// Use this for initialization
	void Start () {
        dir = new Vector2(-1 + 2 * Random.value, -1 + 2 * Random.value).normalized;
        speed = rt.sizeDelta.x;
        StartCoroutine(Go());
	}

    IEnumerator Go() {
        Color c = img.color;
        while (c.a > 0) {
            rt.anchoredPosition += speed * dir;
            c.a -= 0.05f;
            img.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }
	
	
}
