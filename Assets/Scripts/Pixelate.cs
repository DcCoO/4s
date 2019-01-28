using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Pixelate : MonoBehaviour {

    public int pixelDensity;
    // Use this for initialization
    void Update() {
        var material = gameObject.GetComponent<Image>().material;
        Vector2 aspectRatioData;
        if (Screen.height > Screen.width)
            aspectRatioData = new Vector2((float)Screen.width / Screen.height, 1);
        else
            aspectRatioData = new Vector2(1, (float)Screen.height / Screen.width);
        material.SetVector("_AspectRatioMultiplier", aspectRatioData);
        material.SetInt("_PixelDensity", pixelDensity);
    }

    
}