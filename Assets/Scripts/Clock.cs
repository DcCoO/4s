using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour {
    public RectTransform tick;
    public float time = 0;
    public Image fill;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tick.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 360), Vector3.zero, time);
        fill.fillAmount = Mathf.Lerp(0, 1, time);
        time += Time.deltaTime / 60f;

        if(time > 1) {
            StartCoroutine(TrialManager.instance.EndGame());
            transform.SetAsLastSibling();
        }
	}

    public void RewindTime() {
        time = Mathf.Max(0, time - 0.1f);
    }
}
