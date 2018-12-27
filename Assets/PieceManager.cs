using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PieceManager : MonoBehaviour {

    public static PieceManager instance;

    public GameObject piece;
    public GameObject moveArea;

    private List<GameObject> pieces;


    private void Awake() {
        instance = this;
    }

    public void FirstSpawn() {
        pieces = new List<GameObject>();
        float[] xv = { -0.5f, 0.5f };
        float[] yv = { -0.5f, 0.5f };
        for (int i = 0; i < 2; i++) {
            for(int j = 0; j < 2; j++) {
                pieces.Add(Instantiate(piece, moveArea.transform));
                pieces[2 * i + j].rectTransform().anchoredPosition = new Vector2(
                    xv[i] * (moveArea.rectTransform().rect.width / 2),
                    yv[j] * (moveArea.rectTransform().rect.height / 2)
                );
                pieces[2 * i + j].GetComponent<PieceBehaviour>().Init(moveArea.rectTransform(), CheckContact);
            }
        }
    }

    public void Spawn(Vector2 pos, int value) {
        
    }

    private volatile bool checking = false;
    public void CheckContact() {
        if (checking) return;
        checking = true;
        int a = -1, b = -1;
        float minDist = 99999999f, dist;
        for(int i = 0; i < pieces.Count - 1; i++) {
            for(int j = 1; j < pieces.Count; j++) {
                if(RectOverlaps(pieces[i].rectTransform(), pieces[j].rectTransform())){
                    dist = (pieces[i].rectTransform().anchoredPosition - pieces[i].rectTransform().anchoredPosition).magnitude;
                    if(dist < minDist) {
                        minDist = dist;
                        a = i; b = j;
                    }
                }
            }
        }
        if(a != -1) {
            print($"{a} is overlapping {b}");
        }
        checking = false;
    }

    public bool RectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2) {
        return rectTrans1.rect.Overlaps(rectTrans2.rect);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[CustomEditor(typeof(PieceManager))]
public class PieceManagerEditor: Editor {

    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();

        PieceManager pm = target as PieceManager;

        if (GUILayout.Button("Spawn")) {
            pm.FirstSpawn();
        }
    }
}
