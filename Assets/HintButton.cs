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
        level = LevelManager.instance.level;
    }
    
    //TODO fix update hint, not changing when level passes
    public void UpdateHint() {
        hasHint = false;
        videoColor = videoImage.color;
        hintIndex = Memory.GetHintIndex(Memory.GetPlayLevel(), hintNum);
        if (hintIndex > 2) { ActivateHint(); }
    }

    private Color videoColor;
    private bool videoOver = false;

    // Update is called once per frame
    void Update() {
        if(level != LevelManager.instance.level) {
            level = LevelManager.instance.level;
            UpdateHint();
        }
        hintButton.interactable = AdManager.instance.isLoaded;
        if (hintButton.IsInteractable()) videoImage.color = videoColor;
        else videoImage.color = Color.white;

        if (videoOver) ActivateHint();
    }

    //hint 0: operation number
    //hint 1: last 2 numbers
    //hint 2: written hint
    public void ActivateHint() {
        print("ENTROU E TEM HINT? " + hasHint);
        if (hasHint) return;
        int index = hintIndex % 3;
        if (index == 0) {
            print($"Entrando no if no botao {hintNum} com index {index} no level {Memory.GetPlayLevel()}");
            hintText.text = $"The best solution has {LevelManager.best[Memory.GetPlayLevel()]} operations.";
        }
        else {
            print($"Entrando no else no botao {hintNum} com index {index} no level {Memory.GetPlayLevel()}");
            hintText.text = $"The best solution uses {LevelManager.numbers[Memory.GetPlayLevel(), 0]} and {LevelManager.numbers[Memory.GetPlayLevel(), 1]}.";
        }
        hint.SetActive(true);
        button.SetActive(false);
        hasHint = true;
        //memory stuff
        Memory.SetHintIndex(LevelManager.instance.level, hintNum);
    }
}
