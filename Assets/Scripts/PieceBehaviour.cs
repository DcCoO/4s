using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PieceBehaviour : MonoBehaviour {

    public RectTransform pieceImage;
    public Text text;
    public GameObject shadow;
    public RectTransform moveArea;

    [HideInInspector] public RectTransform rt;
    private Vector2 mousePos;

    [HideInInspector] public Action<GameObject> touchCallback, releaseCallback;
    private Vector2 bounds;

    //IMPORTANT INFORMATION:
    [HideInInspector] public int value;
    [HideInInspector] public bool pure;

    public GameObject brokenPiece;

    public void Init(RectTransform area, Action<GameObject> touchCB, Action<GameObject> releaseCB, int value, bool pure) {
        moveArea = area;
        touchCallback = touchCB;
        releaseCallback = releaseCB;
        rt = GetComponent<RectTransform>();
        bounds = new Vector2(
            (moveArea.rect.width - rt.rect.width)/2,
            (moveArea.rect.height - rt.rect.height)/2
        );
        StartCoroutine(Born());
        text.text = value.ToString();

        //important
        this.value = value;
        this.pure = pure;
    }    

    public void TouchDown() {
        transform.SetAsLastSibling();
        mousePos = Input.mousePosition;
        StartCoroutine(IncreaseScale());
        touchCallback(gameObject);
    }

    public void TouchDrag() {
        Vector2 currPos = Input.mousePosition;
        Vector2 diff = currPos - mousePos;
        
        rt.anchoredPosition = new Vector2(
            Mathf.Clamp(rt.anchoredPosition.x + diff.x, -bounds.x, bounds.x),
            Mathf.Clamp(rt.anchoredPosition.y + diff.y, -bounds.y, bounds.y)
        );
        mousePos = currPos;
    }

    public void TouchUp() {
        StartCoroutine(DecreaseScale());
        releaseCallback(gameObject);
    }

    public IEnumerator Born() {
        float duration = 0.1f;
        float porc = 0;
        while (rt.localScale.x < 1) {
            rt.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, porc);
            porc += duration;
            yield return null;
        }
    }

    public void Explode() {
        for(int i = 0; i < 30; i++) {
            Instantiate(brokenPiece, moveArea);
        }
        Destroy(gameObject);
    }

    public void Kill() {
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        float duration = 0.1f;
        float porc = 0;
        while (rt.localScale.x > 0) {
            rt.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, porc);
            porc += duration;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator IncreaseScale() {
        float duration = 0.1f;
        float porc = 0;
        Color c = shadow.GetComponent<Image>().color;
        while (pieceImage.localScale.x < 1.25f) {
            pieceImage.localScale = Vector2.Lerp(Vector2.one, 1.25f * Vector2.one, porc);
            shadow.rectTransform().localScale = Vector2.Lerp(0.8f * Vector2.one, 1.1f * Vector2.one, porc);
            c.a = Mathf.Lerp(0, 0.3f, porc);
            shadow.GetComponent<Image>().color = c;
            porc += duration;
            yield return null;
        }
    }

    IEnumerator DecreaseScale() {
        float duration = 0.1f;
        float porc = 0;
        Color c = shadow.GetComponent<Image>().color;
        while (pieceImage.localScale.x > 1) {
            pieceImage.localScale = Vector2.Lerp(1.25f * Vector2.one, Vector2.one, porc);
            shadow.rectTransform().localScale = Vector2.Lerp(1.1f * Vector2.one, 0.8f * Vector2.one, porc);
            c.a = Mathf.Lerp(0.3f, 0, porc);
            shadow.GetComponent<Image>().color = c;
            porc += duration;
            yield return null;
        }
        pieceImage.localScale = Vector2.one;
    }
}