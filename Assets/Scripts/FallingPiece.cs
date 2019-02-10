using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingPiece : MonoBehaviour {

    public RectTransform rt;
    public Text value;
    public GameObject brokenPiece;

    private Transform t;
    private RectTransform parent;
	// Use this for initialization
	void Start () {
        t = transform;
        value.text = $"{new System.Random().Next(1, 101)}";
        parent = rt.parent as RectTransform;
        float size = 0.08f * parent.rect.width + (Random.value * (parent.rect.width * 0.2f));
        rt.sizeDelta = size * Vector2.one;

        rt.anchoredPosition = new Vector2(
            parent.rect.width * (-0.5f + Random.value),
            (parent.rect.height / 2 + rt.rect.height)
        );
        float speed = 4f + Random.value * 4f;
        float rotSpeed = 20 + Random.value * 300;
        if (Random.value < 0.5) rotSpeed *= -1;

        StartCoroutine(Fall(speed, rotSpeed));
	}
	

    IEnumerator Fall(float speed, float rotSpeed) {
        Vector2 begin = rt.anchoredPosition;
        Vector2 end = new Vector2(rt.anchoredPosition.x, -parent.rect.height / 2 - rt.rect.height);
        for(float i = 0; i <= 1.05f; i += Time.deltaTime / speed) {

            rt.anchoredPosition = Vector2.Lerp(begin, end, i);
            t.Rotate(0, 0, rotSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Explode() {
        for (int i = 0; i < 30; i++) {
            GameObject g = Instantiate(brokenPiece, parent);
            g.rectTransform().anchoredPosition = rt.anchoredPosition;
            float sz = (0.1f + Random.value * 0.3f) * rt.sizeDelta.x;
            g.rectTransform().sizeDelta = sz * Vector2.one;
        }
        Destroy(gameObject);
    }
}
