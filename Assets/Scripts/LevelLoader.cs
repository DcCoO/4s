using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader instance;

    public RectTransform parent;
    public GameObject level;

    void Awake() {
        instance = this;
    }

	void Start () {
        int currentLevel = Memory.GetCurrentLevel();
        bool[] stars = Memory.GetStars();

        for(int i = 1; i <= 100; i++) {
            GameObject g = Instantiate(level, parent);
            g.GetComponent<LevelPiece>().Init(i, i <= currentLevel, stars[i]);
        }

        parent.anchoredPosition = new Vector2(parent.anchoredPosition.x, -10000);
	}

    public void ReturnHome() {
        SceneManager.LoadScene("Main");
    }
	
}
