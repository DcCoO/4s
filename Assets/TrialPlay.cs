using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrialPlay : MonoBehaviour {

    public Button button;
    public Image videoImage;

    private Color videoColor;
    private bool videoOver = false;

    private void Start() {
        videoColor = videoImage.color;

        AdManager.instance.Set(null, null, GoTrial);
    }
    // Update is called once per frame
    void Update () {
        button.interactable = AdManager.instance.isLoaded;
        if (button.IsInteractable()) videoImage.color = videoColor;
        else videoImage.color = Color.white;

        if(videoOver) SceneManager.LoadScene("Trial");
    }

    public void Play() {
        AdManager.instance.ShowAd();
    }

    public void GoTrial() {
        videoOver = true;
    }
}
