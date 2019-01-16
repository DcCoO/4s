using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController instance;
    public int level;

    private void Awake() {
        instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static int[] best = { 0,
        1, 3, 3, 3, 3,
        3, 2, 3, 3, 2,   //10
        3, 2, 3, 4, 2,
        3, 3, 3, 4, 3,   //20
        3, 3, 4, 3, 4,
        3, 4, 2, 4, 3,   //30
        4, 3, 4, 4, 3,
        2, 4, 3, 4, 3,   //40
        4, 3, 2, 2, 2,
        3, 4, 3, 4, 3,   //50
        4, 2, 5, 4, 3,
        4, 5, 3, 5, 2,   //60
        4, 3, 3, 3, 3,
        4, 5, 3, 5, 3,   //70
        5, 3, 5, 4, 5,
        4, 5, 3, 5, 3,   //80
        3, 4, 5, 3, 5,
        4, 5, 1, 5, 4,   //90
        4, 3, 6, 5, 4,
        3, 4, 4, 4, 4    //100
    };
}
