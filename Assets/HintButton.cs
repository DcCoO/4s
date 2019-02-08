using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour {

    public GameObject hint;
    public GameObject button;

    public Image videoImage;
    public Button hintButton;

    public int hintNum;
    public Text hintText;

    private bool hasHint = false;
    private int hintIndex;

    private int level;

    private void Start() {
        //print(Memor)
        videoColor = videoImage.color;
        level = Memory.GetPlayLevel();
        hintIndex = Memory.GetHintIndex(level, hintNum);
        //print($"[START] hintIndex do botao {hintNum} eh {hintIndex} (level = {level})");
        if (hintIndex > 2) UnlockHint();
    }
    
    //TODO fix update hint, not changing when level passes
    public void UpdateHint() {
        hasHint = false;
        button.SetActive(true);
        hint.SetActive(false);
        videoImage.color = videoColor;
        hintIndex = Memory.GetHintIndex(Memory.GetPlayLevel(), hintNum);
        //print($"[UPDATE] hintIndex do botao {hintNum} eh {hintIndex} (level = {level}");
        if (hintIndex > 2) { UnlockHint(); }
    }

    private Color videoColor;
    private bool videoOver = false;

    // Update is called once per frame
    void Update() {

        if(level != LevelManager.instance.level) {
            //print($"BOTAO {hintNum} CHAMOU UPDATE: {level} vs {LevelManager.instance.level}");
            level = LevelManager.instance.level;
            UpdateHint();
        }
        hintButton.interactable = AdManager.instance.isLoaded;
        if (hintButton.IsInteractable()) videoImage.color = videoColor;
        else videoImage.color = Color.white;

        if (videoOver) UnlockHint();
    }

    //hint 0: operation number
    //hint 1: last 2 numbers
    //hint 2: written hint
    public void ActivateHint() {
        AdManager.instance.Set(null, null, UnlockHint);
        AdManager.instance.ShowAd();
        //UnlockHint();
    }

    void UnlockHint() {
        if (hasHint) return;
        int index = hintIndex % 3;
        if (index == 0) {
            hintText.text = $"The best solution has {LevelManager.best[Memory.GetPlayLevel()]} operations.";
        }
        else if (index == 1) {
            hintText.text = $"The best solution uses {LevelManager.numbers[Memory.GetPlayLevel(), 0]} and {LevelManager.numbers[Memory.GetPlayLevel(), 1]}.";
        }
        else {
            hintText.text = LevelManager.hints[Memory.GetPlayLevel()];
        }
        hint.SetActive(true);
        button.SetActive(false);
        hasHint = true;
        //memory stuff
        Memory.SetHintIndex(LevelManager.instance.level, hintNum);
    }
}
