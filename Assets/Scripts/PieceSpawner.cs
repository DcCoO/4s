using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceSpawner : MonoBehaviour {

    public GameObject piece;
    private Transform t;
	// Use this for initialization
	void Start () {
        t = transform;
        StartCoroutine(Spawn());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play() {
        SceneManager.LoadScene("Levels");
    }

    public void PlayFriend() {
        SceneManager.LoadScene("Ranking");
    }

    IEnumerator Spawn() {
        while (true) {
            Instantiate(piece, t);
            yield return new WaitForSeconds(0.3f + Random.value * 1.3f);
        }
    }
}
