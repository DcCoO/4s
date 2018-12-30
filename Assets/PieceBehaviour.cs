using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Callback = System.Action;

public class PieceBehaviour : MonoBehaviour {

    public RectTransform pieceImage;
    public Text text;
    public GameObject shadow;
    public RectTransform moveArea;

    private RectTransform rt;
    private Vector2 mousePos;

    public Callback bindCallback, dieCallback;
    private Vector2 bounds;
    //private Canvas canvas;

    public void Init(RectTransform area, Callback bindCb) {
        //canvas = GetComponent<Canvas>();
        //canvas.overrideSorting = true;
        shadow.GetComponent<Canvas>().overrideSorting = true;
        shadow.GetComponent<Canvas>().sortingOrder = (int)Layer.SHADOW;
        pieceImage.GetComponent<Canvas>().overrideSorting = true;
        pieceImage.GetComponent<Canvas>().sortingOrder = (int)Layer.PIECE;
        //text.GetComponent<Canvas>().overrideSorting = true;
        moveArea = area;
        bindCallback = bindCb;
        rt = GetComponent<RectTransform>();
        bounds = new Vector2(
            (moveArea.rect.width - rt.rect.width)/2,
            (moveArea.rect.height - rt.rect.height)/2
        );
    }
    
    public void SetValue(int value) {
        //text.text = value.ToString();
    }

    public void TouchDown() {
        mousePos = Input.mousePosition;
        StartCoroutine(IncreaseScale());

        pieceImage.GetComponent<Canvas>().sortingOrder = (int) Layer.MOVING_PIECE;
        //text.GetComponent<Canvas>().sortingOrder = (int) Layer.MOVING_PIECE;

        
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
        bindCallback();
    }

    IEnumerator IncreaseScale() {
        float duration = 0.1f;
        float porc = 0;
        Color c = shadow.GetComponent<Image>().color;
        while (pieceImage.localScale.x < 1.25f) {
            pieceImage.localScale = Vector2.Lerp(Vector2.one, 1.25f * Vector2.one, porc);
            shadow.rectTransform().localScale = Vector2.Lerp(Vector2.one, 1.1f * Vector2.one, porc);
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
            shadow.rectTransform().localScale = Vector2.Lerp(1.1f * Vector2.one, Vector2.one, porc);
            c.a = Mathf.Lerp(0.3f, 0, porc);
            shadow.GetComponent<Image>().color = c;
            porc += duration;
            yield return null;
        }
        pieceImage.GetComponent<Canvas>().sortingOrder = (int)Layer.PIECE;
        //text.GetComponent<Canvas>().sortingOrder = (int)Layer.PIECE;
        pieceImage.localScale = Vector2.one;
    }
}