using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (GridLayoutGroup))]
public class GridResizer : MonoBehaviour {
    
    private GridLayoutGroup grid;

    // Use this for initialization
    void Start() {
        grid = GetComponent<GridLayoutGroup>();

        grid.padding.left = grid.padding.right = RuleOf3(grid.padding.left);
        grid.padding.top = RuleOf3(grid.padding.top); grid.padding.bottom = RuleOf3(grid.padding.bottom);
        print("size get to " + grid.cellSize + " at screen width " + Screen.width);
        grid.cellSize = new Vector2(RuleOf3((int)grid.cellSize.x), RuleOf3((int)grid.cellSize.y));
        print("size set to " + grid.cellSize + " at screen width " + Screen.width);
        grid.spacing = new Vector2(RuleOf3((int)grid.spacing.x), RuleOf3((int)grid.spacing.y));
    }
    

    int RuleOf3(int n) {
        return (n * Screen.width) / 600;
    }

    //n - 600
    //x - w

}
