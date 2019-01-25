using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour {

    public Image photo;
    public Text nickname;
    public string ID;
	// Use this for initialization
	public void Init(Sprite fbPhoto, string fbName, string fbID) {
        photo.sprite = fbPhoto;
        nickname.text = fbName;
        ID = fbID;
    }
}
