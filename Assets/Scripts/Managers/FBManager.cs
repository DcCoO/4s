using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBManager : MonoBehaviour {

    public static FBManager instance = null;
    public GameObject loginButton;

    private void Awake() {
        instance = this;
        if (!FB.IsInitialized) {
            FB.Init(() => {
                if (FB.IsInitialized) FB.ActivateApp();
                else Debug.LogError("Couldn't initialize facebook");
            }, isGameShown => {
                if (!isGameShown) Time.timeScale = 0;
                else Time.timeScale = 1;
            });
        }
        else FB.ActivateApp();
    }

    private void Start() {
        loginButton.SetActive(!FB.IsLoggedIn);
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitUntil(() => FB.IsLoggedIn);
        loginButton.SetActive(false);
        StartCoroutine(StoreMyUserID());
        
        GetFriendsPlaying();
    }

    IEnumerator StoreMyUserID() {
        yield return new WaitUntil(() => FB.IsLoggedIn);
        string query = "/me";
        FB.API(query, HttpMethod.GET, result => {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            Memory.SetFacebookName((string)dictionary["name"]);
            Memory.SetFacebookID((string)dictionary["id"]);
            FirebaseManager.instance.AddUser((string)dictionary["id"], (string)dictionary["name"]);
        });
    }

    public void Login() {
        var permissions = new List<string>() { "user_friends" };
        FB.LogInWithReadPermissions(permissions);
        StartCoroutine(StoreMyUserID());
    }

    public void Logout() {
        FB.LogOut();
    }

    public void Share() {
        FB.ShareLink(
            new System.Uri("http://www.google.com"),
            "Check it out",
            "Good things to see",
            new System.Uri("https://ih0.redbubble.net/image.427755805.9781/flat,1000x1000,075,f.u2.jpg")
        );
    }

    public void GameRequest() {
        FB.AppRequest("Hey! Come and play Quatros!", title: "Quatros");
    }

    public void Invite() {
        FB.Mobile.AppInvite(
            new System.Uri("link do jogo na Play Store")
        );
    }

    public Dictionary<string, FBFriend> friends;

    public void GetFriendsPlaying() {

        friends = new Dictionary<string, FBFriend>();

        string query = "/me/friends";

        FB.API(query, HttpMethod.GET, result => {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];


            foreach (var dict in friendsList) {
                string name = (string)((Dictionary<string, object>)dict)["name"];
                string id = (string)((Dictionary<string, object>)dict)["id"];
               
                FBFriend fbf = new FBFriend();
                fbf.name = name; fbf.id = id;
                friends[id] = fbf;
                getSprite(id);
            }

            FBFriend me = new FBFriend();
            me.name = Memory.GetFacebookName(); me.id = Memory.GetFacebookID(); me.score = Memory.GetScore();
            friends[me.id] = me;
            getSprite(me.id);

            FirebaseManager.instance.GetFriends();
        });
    }

    public void getSprite(string id) {
        FB.API("https://graph.facebook.com/" + id + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result) {
            friends[id].photo = Sprite.Create(result.Texture, new Rect(Vector2.zero,
            new Vector2(result.Texture.width, result.Texture.height)), Vector2.zero);
            FriendLoader.instance.LoadPhoto(friends[id].photo, id);
        });
    }
    
}

public class FBFriend {
    public string name, id;
    public int score;
    public Sprite photo = null;
    public FBFriend() { }
}
