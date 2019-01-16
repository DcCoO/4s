using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operator : MonoBehaviour {

    public Operation operation;
    private Button btn;

    private PieceBehaviour v1, v2;

    public void Activate(PieceBehaviour p1, PieceBehaviour p2) {
        v1 = p1;
        v2 = p2;

        if (btn == null) btn = GetComponent<Button>();
        btn.interactable = MathSolver.CanSolve(operation, v1.value, v2.value);
        if(operation == Operation.CONCAT) {
            
            btn.interactable = v1.pure && v2.pure && btn.interactable;
        }
    }

    public void Operate() {
        ActionManager.instance.SaveState(operation);
        PieceManager.instance.Spawn(v1, v2, operation);
        OperationController.instance.Hide();
        LevelManager.instance.CheckResult();
    }
    
}
