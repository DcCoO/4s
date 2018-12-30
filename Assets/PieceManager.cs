using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PieceManager : MonoBehaviour {

    public static PieceManager instance;

    public GameObject piece;
    public GameObject patternPiece;
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
                pieces[2 * i + j].rectTransform().sizeDelta = new Vector2(
                    patternPiece.rectTransform().rect.height,
                    patternPiece.rectTransform().rect.height
                );
                pieces[2 * i + j].GetComponent<PieceBehaviour>().Init(moveArea.rectTransform(), CheckContact);
                pieces[2 * i + j].name = "Piece " + (2 * i + j).ToString();
            }
        }
        
    }

    public void Fix() {
        for(int i = 0; i < 4; i++) {
            pieces[i].rectTransform().sizeDelta = new Vector2(
                patternPiece.rectTransform().rect.height,
                patternPiece.rectTransform().rect.height
            );  
            continue;
            pieces[i].rectTransform().rect.Set(
                pieces[i].rectTransform().rect.x,
                pieces[i].rectTransform().rect.y,
                patternPiece.rectTransform().rect.height,
                patternPiece.rectTransform().rect.height
            );
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
            for(int j = i + 1; j < pieces.Count; j++) {
                if(RectOverlaps(pieces[i].rectTransform(), pieces[j].rectTransform())){
                    dist = DistBetweenRects(pieces[i].rectTransform(), pieces[j].rectTransform());
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

    public float DistBetweenRects(RectTransform rectTrans1, RectTransform rectTrans2) {
        Rect r1 = GetWorldSpaceRect(rectTrans1);
        Rect r2 = GetWorldSpaceRect(rectTrans2);
        return (r1.position - r2.position).magnitude;
    }

    public bool RectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2) {
        return GetWorldSpaceRect(rectTrans1).Overlaps(GetWorldSpaceRect(rectTrans2));
    }

    Rect GetWorldSpaceRect(RectTransform rt) {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
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
    bool spawn = false;
    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();

        PieceManager pm = target as PieceManager;

        if (GUILayout.Button("Spawn")) {
            if (!spawn) {
                pm.FirstSpawn();
                spawn = true;
            }
            else pm.Fix();
        }
    }
}
