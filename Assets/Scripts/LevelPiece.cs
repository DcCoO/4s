using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPiece : MonoBehaviour {

    public Image piece, star;
    public Text number;
    [HideInInspector] public int value;

    private bool locked;

    public Sprite lockedPiece, freePiece, starHole, starReal;
	
    public void Init(int val, bool open, bool hasStar) {
        locked = !open;
        if (open) {
            number.text = val.ToString();
            value = val;
            piece.sprite = freePiece;
            if (hasStar) {
                star.sprite = starReal;
                star.color = Color.white;
            }
            else star.sprite = starHole;
        }
        else {
            value = val;
            number.enabled = false;
            piece.sprite = lockedPiece;
            star.color = Color.clear;
        }
    }

    public void Run() {
        if (!locked) {
            Memory.SetPlayLevel(value);
            SceneManager.LoadScene("Game");
        }
    }
}
