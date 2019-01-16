using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PieceManager : MonoBehaviour {

    public static PieceManager instance;

    public GameObject piece;
    public GameObject patternPiece;
    public GameObject moveArea;
    public RectTransform star;

    [HideInInspector] public List<GameObject> pieces = new List<GameObject>();


    private void Awake() {
        instance = this;
    }

    public List<Piece> GetPieces() {
        List<Piece> ps = new List<Piece>();
        foreach(GameObject g in pieces) {
            PieceBehaviour pb = g.GetComponent<PieceBehaviour>();
            //print("(" + pb.value + ", " + pb.pure + ")");
            ps.Add(new Piece(pb.value, pb.pure));
        }
        return ps;
    }
    

    public void FirstSpawn() {
        
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
                pieces[2 * i + j].GetComponent<PieceBehaviour>().Init(moveArea.rectTransform(), BeginDrag, EndDrag, 4, true);
                //pieces[2 * i + j].name = "Piece " + (2 * i + j).ToString();
            }
        }
        
    }

    public void Spawn(int value, bool pure, Vector2 position) {
        pieces.Add(Instantiate(piece, moveArea.transform));
        pieces[pieces.Count - 1].rectTransform().anchoredPosition = position;
        pieces[pieces.Count - 1].rectTransform().sizeDelta = new Vector2(
            patternPiece.rectTransform().rect.height,
            patternPiece.rectTransform().rect.height
        );
        pieces[pieces.Count - 1].GetComponent<PieceBehaviour>().Init(moveArea.rectTransform(), BeginDrag, EndDrag, value, pure);
    }

    public void Clear() {
        foreach (GameObject g in pieces) Destroy(g);
        pieces.Clear();
    }

    public void Spawn(PieceBehaviour v1, PieceBehaviour v2, Operation op) {
        int i1 = -1, i2 = -1;
        for(int i = 0; i < pieces.Count; i++) {
            if (pieces[i] == v1.gameObject) i1 = i;
            if (pieces[i] == v2.gameObject) i2 = i;
        }

        int value = MathSolver.Solve(op, v1.value, v2.value);
        bool pure = (op == Operation.CONCAT);

        if(i1 == i2) {
            GameObject g1 = pieces[i1];
            pieces.Remove(g1);
            g1.GetComponent<PieceBehaviour>().Kill();
        }
        else {
            GameObject g1 = pieces[i1], g2 = pieces[i2];
            pieces.Remove(g1); pieces.Remove(g2);
            g1.GetComponent<PieceBehaviour>().Kill();
            g2.GetComponent<PieceBehaviour>().Kill();
        }

        Spawn(value, pure, (v1.rt.anchoredPosition + v2.rt.anchoredPosition) / 2);
    }

    public void Fix() {
        for(int i = 0; i < 4; i++) {
            pieces[i].rectTransform().sizeDelta = new Vector2(
                patternPiece.rectTransform().rect.height,
                patternPiece.rectTransform().rect.height
            );
        }
    }
    
    Vector2 beginPos;

    public void BeginDrag(GameObject g) {
        OperationController.instance.Hide();
        beginPos = g.rectTransform().anchoredPosition;
    }

    public void EndDrag(GameObject g) {
        bool unary = (g.rectTransform().anchoredPosition - beginPos).magnitude < 0.1f;
        int hit = CheckContact(ref g);
        if (hit != -1) {
            //BINARY operation
            OperationController.instance.ShowBinary(g.GetComponent<PieceBehaviour>(), pieces[hit].GetComponent<PieceBehaviour>());
        }
        else if (unary) {
            //UNARY operation
            OperationController.instance.ShowUnary(g.GetComponent<PieceBehaviour>());
        }
    }

    
    
    public int CheckContact(ref GameObject g) {
        float minDist = 99999999f, dist;
        int index = -1;
        for(int i = 0; i < pieces.Count; i++) {
            if (pieces[i] == g) continue;

            if(RectOverlaps(pieces[i].rectTransform(), g.rectTransform())){
                dist = DistBetweenRects(pieces[i].rectTransform(), g.rectTransform());
                if(dist < minDist) {
                    minDist = dist;
                    index = i;
                }
            }
        }

        return index;
        //print($"{a} is overlapping {b}");
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

    
    public void Throw() {
        pieces[0].GetComponent<PieceBehaviour>().Explode();
    }

    private void Start() {
        star.sizeDelta = new Vector2(
            patternPiece.rectTransform().rect.height,
            patternPiece.rectTransform().rect.height
        );
        star.anchoredPosition = Vector2.zero;
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
                pm.Throw();
                spawn = true;
            }
            else pm.Fix();
        }
    }
}
