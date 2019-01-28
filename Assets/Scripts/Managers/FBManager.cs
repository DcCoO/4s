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
            print("Nome: " + (string)dictionary["name"]);
            print("Nome: " + (string)dictionary["id"]);
            FirebaseManager.instance.AddUser((string)dictionary["id"], (string)dictionary["name"], Memory.GetScore());
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

    public bool searching = false;
    Dictionary<string, FBFriend> dt;

    public void GetFriendsPlaying() {
        searching = false;

        dt = new Dictionary<string, FBFriend>();

        string query = "/me/friends";
        
        FB.API(query, HttpMethod.GET, result => {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            foreach (var dict in friendsList) {
                string name = (string)((Dictionary<string, object>)dict)["name"];
                string id = (string)((Dictionary<string, object>)dict)["id"];

                FBFriend fbf = new FBFriend();
                fbf.name = name; fbf.id = id;
                dt[id] = fbf;
                getSprite((Dictionary<string, object>)dict, id);
                //FriendLoader.instance.friends[0].nickname.text = (string)((Dictionary<string, object>)dict)["name"];
                //FriendLoader.instance.friends[0].ID = (string)((Dictionary<string, object>)dict)["id"];
            }
            searching = false;
        });
    }

    void getSprite(Dictionary<string, object> user, string id) {
        FB.API("https://graph.facebook.com/" + user["id"] + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result) {

            //FriendLoader.instance.friends[i].photo.sprite = Sprite.Create(result.Texture, new Rect(Vector2.zero,
            //new Vector2(result.Texture.width, result.Texture.height)), Vector2.zero);
            dt[id].photo = Sprite.Create(result.Texture, new Rect(Vector2.zero,
            new Vector2(result.Texture.width, result.Texture.height)), Vector2.zero);

            //FriendLoader.instance.friends[0].SetImage(Sprite.Create(result.Texture, new Rect(Vector2.zero,
            //new Vector2(result.Texture.width, result.Texture.height)), Vector2.zero));
        });
    }
    
}

public class FBFriend {
    public string name, id;
    public Sprite photo = null;
    public FBFriend() { }
}
