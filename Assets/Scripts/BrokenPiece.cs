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
        rt.sizeDelta *= (0.7f + Random.value);
        StartCoroutine(Go());
	}

    IEnumerator Go() {
        Color s = img.color;
        Color e = img.color; e.a = 0;
        for (float i = 0; i <= 1.05f; i += Time.deltaTime * 2f) {
            img.color = Color.Lerp(s, e, i);
            rt.anchoredPosition += speed * dir;
            yield return null;
        }
        Destroy(gameObject);
    }
	
	
}
