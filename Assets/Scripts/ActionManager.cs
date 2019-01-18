using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour {

    public static ActionManager instance;

    //ref para botoes
    public Button undo, restart, back;

    [HideInInspector] public Stack<Operation> operations = new Stack<Operation>();
    [HideInInspector] public Stack<GameState> states = new Stack<GameState>();
    // Use this for initialization
    
    void Awake() {
        instance = this;
    }
    public void SaveState(Operation op) {
        undo.interactable = true;
        operations.Push(op);
        states.Push(new GameState(PieceManager.instance.GetPieces()));
    }

    /*private void Start() {
        List<Piece> list = new List<Piece>();
        for (int i = 0; i < 4; i++) list.Add(new Piece(4, true));
        states.Add(new GameState(list));

    }*/

    public void Restart() {
        PieceManager.instance.Clear();
        OperationController.instance.Hide();
        Clear();
        undo.interactable = false;
        Rect r = PieceManager.instance.moveArea.rectTransform().rect;
        PieceManager.instance.Spawn(4, true, new Vector2(-0.25f * r.width, -0.25f * r.height));
        PieceManager.instance.Spawn(4, true, new Vector2(0.25f * r.width, -0.25f * r.height));
        PieceManager.instance.Spawn(4, true, new Vector2(-0.25f * r.width, 0.25f * r.height));
        PieceManager.instance.Spawn(4, true, new Vector2(0.25f * r.width, 0.25f * r.height));
    }

    public void TurnButtons(bool undoState, bool restartState, bool backState) {
        undo.interactable = undoState;
        restart.interactable = restartState;
        back.interactable = backState;
    }

    public void Undo() {

        if (operations.Count == 1) undo.interactable = false;

        Rect r = PieceManager.instance.moveArea.rectTransform().rect;
        operations.Pop();
        GameState gs = states.Pop();

        PieceManager.instance.Clear();
        OperationController.instance.Hide();

        //top mid
        if (gs.size == 2 || gs.size == 3) {
            PieceManager.instance.Spawn(gs.pieces[0].value, gs.pieces[0].pure, new Vector2(0, 0.25f * r.height));
        }
        //bottom mid
        if (gs.size == 2 ) {
            PieceManager.instance.Spawn(gs.pieces[1].value, gs.pieces[1].pure, new Vector2(0, -0.25f * r.height));
        }
        //bottom left, bottom right
        if (gs.size == 3 || gs.size == 4) {
            PieceManager.instance.Spawn(gs.pieces[1].value, gs.pieces[1].pure, new Vector2(-0.25f * r.width, -0.25f * r.height));
            PieceManager.instance.Spawn(gs.pieces[2].value, gs.pieces[2].pure, new Vector2(0.25f * r.width, -0.25f * r.height));
        }
        //top left, top right
        if(gs.size == 4) {
            PieceManager.instance.Spawn(gs.pieces[0].value, gs.pieces[0].pure, new Vector2(-0.25f * r.width, 0.25f * r.height));
            PieceManager.instance.Spawn(gs.pieces[3].value, gs.pieces[3].pure, new Vector2(0.25f * r.width, 0.25f * r.height));
        }
        //center
        if (gs.size == 1) {
            PieceManager.instance.Spawn(gs.pieces[0].value, gs.pieces[0].pure, new Vector2(0, 0));
        }
        
    }

    public int GetNumOp() {
        int r = 0;
        foreach (Operation op in operations) if (op != Operation.CONCAT) r++;
        return r;
    }

    public void Clear() {
        states.Clear();
        operations.Clear();
    }
}

public class GameState {
    public List<Piece> pieces;
    public int size;
    public GameState(List<Piece> list) { pieces = list; size = list.Count; }
}

public class Piece {
    public int value;
    public bool pure;
    public Piece(int v, bool p) { value = v; pure = p; }
}
