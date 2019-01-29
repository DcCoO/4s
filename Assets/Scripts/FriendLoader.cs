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

    public void LoadFriends(List<FBFriend> FBfriends) {

        //FBFriend me = new FBFriend();
        //me.name = Memory.GetFacebookName(); me.id = Memory.GetFacebookID(); me.score = Memory.GetScore();
        //FBfriends.Add(me);
        //FBManager.instance.getSprite(me.id);

        FBfriends.Sort((x, y) => y.score.CompareTo(x.score));

        for(int i = 0; i < Mathf.Min(5, FBfriends.Count); i++) {
            //TODO check for score 0 and dont show if its 0
            friends[i].friend = FBfriends[i];
            friends[i].Init();
        }
    }

    public void LoadPhoto(Sprite photo, string id) {
        StartCoroutine(LoadPhotoRoutine(photo, id));
    }

    IEnumerator LoadPhotoRoutine(Sprite photo, string id) {
        float timeout = 0;
        Friend target = new Friend();
        bool foundFriend = false;
        while (!foundFriend) {
            foreach(Friend f in friends) {
                if(f.friend.id == id) {
                    target = f;
                    target.friend.photo = photo;
                    foundFriend = true;
                    break;
                }
                yield return null;
                timeout += Time.deltaTime;
            }
            if (timeout > 5) break;
        }
        if(foundFriend) target.SetImage(photo);
    }






    public void ReturnHome() {
        SceneManager.LoadScene("Main");
    }
}
