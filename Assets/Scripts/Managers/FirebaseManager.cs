using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Facebook.MiniJSON;

public class FirebaseManager : MonoBehaviour {

    public static FirebaseManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        StartCoroutine(GetFriends());
    }

    public void AddUser(string id, string name, int score) {

    }

    IEnumerator GetFriends(List<FBFriend> list) {
        WWWForm form = new WWWForm();
        foreach(FBFriend fbf in list) form.AddField("ids[]", fbf.id);

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-quatros-9ca45.cloudfunctions.net/get_users", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            Dictionary<string, object> dic = 
                (Dictionary<string, object>) Json.Deserialize(www.downloadHandler.text);

            Dictionary<string, object> d2 = dic["2"] as Dictionary<string, object>;
            print(d2["name"]);
            print(d2["score"]);

        }
    }


    IEnumerator Upload() {
        WWWForm form = new WWWForm();
        form.AddField("email", "DaNieL@email.email");

        

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
