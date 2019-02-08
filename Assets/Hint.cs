using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour {

    public Image background;
    public GameObject hintWindow;

    private bool popping = false;
    private void Awake() {

        //Memory.ForceInitHint();
        Memory.InitHint();
    }

    public void ActivateHintWindow(bool on) {
        if (popping) return;
        StartCoroutine(Pop(on));
    }

    IEnumerator Pop(bool on) {
        popping = true;
        RectTransform rt = hintWindow.rectTransform();
        float time = 0;
        if (on) {
            while (rt.localScale.x < 1) {
                rt.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, time);
                time += Time.deltaTime * 3;
                yield return null;
            }
            background.enabled = on;
        }
        else {
            while (rt.localScale.x > 0) {
                rt.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, time);
                time += Time.deltaTime * 3;
                yield return null;
            }
            background.enabled = on;
        }
        popping = false;
    }

    //TODO codigo inutil pode ser apagado


}
