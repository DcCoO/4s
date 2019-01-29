using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Facebook.MiniJSON;
using System;

public class FirebaseManager : MonoBehaviour {

    public static FirebaseManager instance;

    private void Awake() {
        instance = this;
    }
    

    public void GetFriends() {
        StartCoroutine(GetFriends(FBManager.instance.friends));
    }

    IEnumerator GetFriends(Dictionary<string, FBFriend> dict) {
        WWWForm form = new WWWForm();
        foreach(FBFriend fbf in dict.Values) form.AddField("ids[]", fbf.id);

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-quatros-9ca45.cloudfunctions.net/get_users", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
            Dictionary<string, object> dic = (Dictionary<string, object>) Json.Deserialize(www.downloadHandler.text);

            List<FBFriend> friendList = new List<FBFriend>();
            //{id:{name:value}}
            foreach (KeyValuePair<string, object> entry in dic) {
                // do something with entry.Value or entry.Key
                string id = entry.Key;
                int score = Convert.ToInt32(((Dictionary<string, object>)entry.Value)["score"]);
                //print($"{((Dictionary<string, object>)entry.Value)["name"]} has {score} points");
                if (id == Memory.GetFacebookID()) score = Memory.GetScore();
                //print($"{((Dictionary<string, object>)entry.Value)["name"]} has {score} points");
                FBManager.instance.friends[id].score = score;
                friendList.Add(FBManager.instance.friends[id]);
            }
            
            FriendLoader.instance.LoadFriends(friendList);
        }
    }

    public void AddUser(string id, string name) {
        StartCoroutine(AddUser(id, name, Memory.GetScore()));
    }


    IEnumerator AddUser(string id, string name, int score) {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("name", name);
        form.AddField("score", score); 
        
        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-quatros-9ca45.cloudfunctions.net/add_user", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
        }
    }
}
