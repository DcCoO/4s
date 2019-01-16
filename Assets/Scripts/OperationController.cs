using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationController : MonoBehaviour {

    public static OperationController instance;

    public RectTransform unary, binary;
    public RectTransform shadow;

    private Transform tf;

    void Awake() {
        instance = this;
    }

    private void Start() {
        tf = transform;
    }

    public void ShowUnary(PieceBehaviour piece1) {
        RectTransform r1 = piece1.rt;
        float x = r1.anchoredPosition.x;
        float y = r1.anchoredPosition.y;
        y += (unary.rect.height + r1.rect.height ) / 2;
        unary.anchoredPosition = new Vector2(x, y);        

        // all
        // shadow
        // piece1
        // piece2
        // operations

        shadow.SetAsLastSibling();
        r1.SetAsLastSibling();
        tf.SetAsLastSibling();

        Operator[] os = unary.GetComponentsInChildren<Operator>();
        foreach(Operator o in os) o.Activate(piece1, piece1);

        unary.gameObject.SetActive(true);
        shadow.gameObject.SetActive(true);
    }

    public void ShowBinary(PieceBehaviour piece1, PieceBehaviour piece2) {
        RectTransform r1 = piece1.rt, r2 = piece2.rt;
        float x = (r1.anchoredPosition.x + r2.anchoredPosition.x) / 2;
        float y = Mathf.Max(r1.anchoredPosition.y, r2.anchoredPosition.y);
        y += (binary.rect.height + r1.rect.height) / 2;
        binary.anchoredPosition = new Vector2(x, y);

        // all
        // shadow
        // piece1
        // piece2
        // operations

        shadow.SetAsLastSibling();
        r2.SetAsLastSibling();
        r1.SetAsLastSibling();
        tf.SetAsLastSibling();

        Operator[] os = binary.GetComponentsInChildren<Operator>();
        foreach (Operator o in os) o.Activate(piece1, piece2);

        binary.gameObject.SetActive(true);
        shadow.gameObject.SetActive(true);
    }

    public void Hide() {
        unary.gameObject.SetActive(false);
        binary.gameObject.SetActive(false);
        shadow.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
