using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPiece : MonoBehaviour {

    public Image piece;
    public Text number;
    [HideInInspector] public int value;

    private bool locked;

    public Sprite lockedPiece, normalPiece, starPiece;
	
    public void Init(int val, bool open, bool hasStar) {
        locked = !open;
        if (open) {
            number.text = val.ToString();
            value = val;
            piece.sprite = hasStar ? starPiece : normalPiece;
        }
        else {
            value = val;
            number.enabled = false;
            piece.sprite = lockedPiece;
        }
    }

    public void Run() {
        if (!locked) {
            Memory.SetPlayLevel(value);
            SceneManager.LoadScene("Game");
        }
    }
}
