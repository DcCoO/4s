using UnityEngine;
using UnityEngine.UI;
using Callback = System.Action;

public class PieceBehaviour : MonoBehaviour {

    public Text text;
    public RectTransform moveArea;

    private RectTransform rt;
    private Vector2 mousePos;

    public Callback bindCallback, dieCallback;
    private Vector2 bounds;

    public void Init(RectTransform area, Callback bindCb) {
        moveArea = area;
        bindCallback = bindCb;
        rt = GetComponent<RectTransform>();
        bounds = new Vector2(
            (moveArea.rect.width - rt.rect.width)/2,
            (moveArea.rect.height - rt.rect.height)/2
        );
    }
    
    public void SetValue(int value) {
        text.text = value.ToString();
    }

    public void TouchDown() {
        mousePos = Input.mousePosition;
        print(rt.anchoredPosition);
        print(moveArea.rect.width);
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
        bindCallback();
    }
}