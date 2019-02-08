using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ExtraPiece : MonoBehaviour {

    public static ExtraPiece instance;

    public RectTransform rt, parent;
    public Text value;

    private void Awake() {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        rt.anchoredPosition = new Vector2(parent.rect.width / 2 + rt.rect.height + 0.1f, rt.anchoredPosition.y);
        int playLevel = Memory.GetPlayLevel();
        value.text = $"{playLevel + 1}";
	}
	
    public void MoveOut() {
        rt.anchoredPosition = new Vector2(parent.rect.width / 2 + rt.rect.height + 0.1f, rt.anchoredPosition.y);
        int playLevel = Memory.GetPlayLevel();
        value.text = $"{playLevel + 1}";
    }
	// Update is called once per frame

    public IEnumerator Appear() {
        Vector2 beginPos = rt.anchoredPosition;
        Vector2 endPos = new Vector2(parent.rect.width/2 - 0.25f * rt.rect.height, rt.anchoredPosition.y);
        for (float i = 0; i <= 1.05; i += Time.deltaTime * 2) {
            rt.anchoredPosition = Vector2.Lerp(beginPos, endPos, i);
            yield return null;
        }
    }

    public IEnumerator Disappear() {
        Vector2 beginPos = rt.anchoredPosition;
        Vector2 endPos = new Vector2(parent.rect.width / 2 + rt.rect.height + 0.1f, rt.anchoredPosition.y);
        for (float i = 0; i <= 1.05; i += Time.deltaTime * 2) {
            rt.anchoredPosition = Vector2.Lerp(beginPos, endPos, i);
            yield return null;
        }
    }

    public IEnumerator Replace() {
        Vector2 beginPos = rt.anchoredPosition;
        Vector2 endPos = new Vector2(0, rt.anchoredPosition.y);
        for (float i = 0; i <= 1.05; i += Time.deltaTime * 2) {
            rt.anchoredPosition = Vector2.Lerp(beginPos, endPos, i);
            yield return null;
        }
    }

    public void Click() {
        LevelManager.instance.NextLevel();
    }

	void Update () {
		
	}
}
