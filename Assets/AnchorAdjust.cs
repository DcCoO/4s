using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent (typeof (RectTransform))]
public class AnchorAdjust : MonoBehaviour {

    public RectTransform rt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Adjust() {
        rt = GetComponent<RectTransform>();
        Rect par = ((RectTransform)rt.parent).rect;
        Vector2 offset = new Vector2(par.width/2, par.height/2);

        float leftP = (offset.x + rt.anchoredPosition.x - (rt.rect.width * rt.localScale.x) / 2f) / par.width;
        float rightP = (offset.x + rt.anchoredPosition.x + (rt.rect.width * rt.localScale.x) / 2f) / par.width;
        float topP = (offset.y + rt.anchoredPosition.y + (rt.rect.height * rt.localScale.y) / 2f) / par.height;
        float bottomP = (offset.y + rt.anchoredPosition.y - (rt.rect.height * rt.localScale.y) / 2f) / par.height;

        rt.anchorMax = new Vector2(rightP, topP);
        rt.anchorMin = new Vector2(leftP, bottomP);

        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnchorAdjust))]
public class AnchorAdjustEditor : Editor {
    public override void OnInspectorGUI() {
        AnchorAdjust myTarget = (AnchorAdjust)target;

        if(GUILayout.Button("Adjust anchor")) {
            myTarget.Adjust();
        }
    }
}
#endif