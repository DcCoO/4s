using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendLoader : MonoBehaviour {

    public static FriendLoader instance;

    public Friend[] friends;

    void Awake() {
        instance = this;
    }

    
    

    

    public void ReturnHome() {
        SceneManager.LoadScene("Main");
    }
}
