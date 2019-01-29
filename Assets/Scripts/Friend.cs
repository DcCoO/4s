using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour {

    public Image photo;
    public Text nickname;
    public FBFriend friend;

    //public bool initialized = false;

	public void Init() {
        photo.sprite = friend.photo;
        nickname.text = friend.name + $" ({friend.score})";
        photo.gameObject.SetActive(true);
        print("init with " + friend.name + ", " + friend.score);
    }
    
    public void SetImage(Sprite img) {
        photo.sprite = img;
        photo.gameObject.SetActive(true);
    }

    
}
