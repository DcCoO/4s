using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Upload());
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
