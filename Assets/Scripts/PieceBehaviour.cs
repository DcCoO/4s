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
        shadow.rectTransform().sizeDelta = new Vector2(Screen.width / 6, Screen.width / 6);

        //important
        this.value = value;
        this.pure = pure;
    }    

    public void TouchDown() {
        transform.SetAsLastSibling();
        StartCoroutine(IncreaseScale());
        touchCallback(gameObject);
    }
    
    public void TouchDrag() {
        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(moveArea, Input.mousePosition, Camera.main, out localpoint);
        
        rt.anchoredPosition = new Vector2(
            Mathf.Clamp(localpoint.x, -bounds.x, bounds.x),
            Mathf.Clamp(localpoint.y, -bounds.y, bounds.y)
        );
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
            Instantiate(brokenPiece, moveArea).GetComponent<RectTransform>().anchoredPosition = rt.anchoredPosition;
        }
        Destroy(gameObject);
    }

    public void StarExplode() {
        Sprite star = TrialManager.instance.star.GetComponent<Image>().sprite;
        for (int i = 0; i < 30; i++) {
            GameObject g = brokenPiece;
            g = Instantiate(g, moveArea);
            g.GetComponent<RectTransform>().anchoredPosition = rt.anchoredPosition;
            g.GetComponent<Image>().sprite = star;
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
            shadow.rectTransform().localScale = Vector2.Lerp(Vector2.one, 1.6f * Vector2.one, porc);
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
            shadow.rectTransform().localScale = Vector2.Lerp(1.6f * Vector2.one, Vector2.one, porc);
            c.a = Mathf.Lerp(0.3f, 0, porc);
            shadow.GetComponent<Image>().color = c;
            porc += duration;
            yield return null;
        }
        pieceImage.localScale = Vector2.one;
    }
}