using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBManager : MonoBehaviour {

    public Text FriendsText;
    public Image FriendsImage;

    private void Awake() {
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


    public void Login() {
        var permissions = new List<string>() { "user_friends" };
        FB.LogInWithReadPermissions(permissions);
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

    public void GetFriendsPlaying() {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result => {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            FriendsText.text = string.Empty;
            foreach (var dict in friendsList) {
                FriendsText.text += ((Dictionary<string, object>) dict)["name"];
                getSprite((Dictionary<string, object>) dict);
            } 
        });
    }

    void getSprite(Dictionary<string, object> user) {
        FB.API("https" + "://graph.facebook.com/" + user["id"].ToString() + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result)
        {
            FriendsImage.sprite = Sprite.Create(result.Texture, new Rect(Vector2.zero,
                new Vector2(result.Texture.width, result.Texture.height)), Vector2.zero);
        });
    }
    
}
