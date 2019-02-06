using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class TrialManager : MonoBehaviour {

    public static TrialManager instance;

    public int score;
    public int level;
    public Text phrase;
    public Text levelNumber;
    public RectTransform dialog, undoButton, restartButton, backButton, restartButton2, nextButton, levelPiece;
    public Image shadow, moveArea;
    public GameObject star;
    public Sprite rightBaloon;
    public Image dialogImage;

    bool gotStar = false;
    bool readyToNext = false;
    string[] star_phrase = { "Fantastic!", "Amazing!", "Incredible!", "Brilliant!", "Wonderful!", "Awesome!", "Flawless!" };
    string[] win_phrase = { "Good!", "Nice!", "You did it!", "Very good!", "Great!", "Well done!", "Way to go!" };

    List<int> levels = new List<int>();

    void Awake() {
        instance = this;
    }
    // Use this for initialization
    void Start() {
        for (int i = 0; i < 101; i++) levels.Add(i);
        level = PopRandomLevel();
        score = 0;
        levelNumber.text = level.ToString();
        phrase.text = $"Score: {score}";

    }

    int PopRandomLevel() {
        int r = new System.Random().Next(0, levels.Count);
        int ans = levels[r];
        levels.RemoveAt(r);
        return ans;
    }

    // Update is called once per frame
    public void CheckResult() {
        if (PieceManager.instance.pieces.Count == 1) {
            if (PieceManager.instance.pieces[0].GetComponent<PieceBehaviour>().value == level) {
                //ActionManager.instance.TurnButtons(false, false, false);
                StartCoroutine(NextNumber());
            }
        }
    }

    void DoNothing(GameObject g) { }

    IEnumerator NextNumber() {

        gotStar = ActionManager.instance.GetNumOp() == best[level];

        PieceBehaviour pb = PieceManager.instance.pieces[0].GetComponent<PieceBehaviour>();

        yield return new WaitUntil(() => pb.rt.localScale.x > 0.9f);
        yield return new WaitForSeconds(0.5f);

        if (gotStar) {
            score += 3;
            pb.StarExplode();
        }
        else {
            pb.Explode();
            score += 2;
        }

        level = PopRandomLevel();
        levelNumber.text = level.ToString();
        phrase.text = $"Score: {score}";
        ActionManager.instance.Restart();
    }
    

    private Sprite oldSprite;
    public IEnumerator EndGame() {

        float time = 0;

        for(int i = 0; i < PieceManager.instance.pieces.Count; i++) {
            PieceManager.instance.pieces[i].GetComponent<PieceBehaviour>().Explode();
            yield return null;
        }

        Color c1 = shadow.color;
        while(time < 1) {
            c1.a = Mathf.Lerp(0, 0.7f, time); shadow.color = c1;
            time += Time.deltaTime;
            yield return null;
        }

        //store in firebase
        Memory.SetScore(score);
        FirebaseManager.instance.AddUser(Memory.GetFacebookID(), Memory.GetFacebookName());

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Ranking");
    }

    public void ResetLevel() {
        if (readyToNext) {
            readyToNext = false;
            //Memory.SetPlayLevel(++level);
            StartCoroutine(PrepareSameLevel());
        }
    }


    IEnumerator PrepareSameLevel() {
        float time = 30;
        for (float i = 0; i <= time; i++) {
            nextButton.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            restartButton2.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            dialog.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            //levelPiece.anchoredPosition = Vector2.Lerp(sp, ep, i / time);
            star.rectTransform().localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //some com sombra, volta botoes antigos, volta balao antigo, seta novo level, ativa ponta
        //arrow.SetActive(true);
        dialogImage.sprite = oldSprite;
        phrase.text = $"Can you turn these 4 pieces into {level}?";
        Color c1 = shadow.color; Color c2 = moveArea.color;
        for (float i = 0; i <= time; i++) {
            restartButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            undoButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            dialog.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            c2.a = Mathf.Lerp(0, 1, i / time); moveArea.color = c2;
            c1.a = Mathf.Lerp(0.7f, 0, i / time); shadow.color = c1;
            yield return null;
        }

        star.SetActive(false);
        star.rectTransform().localScale = Vector2.one;
        ActionManager.instance.Restart();
    }

    public void ReturnHome() {
        SceneManager.LoadScene("Ranking");
    }




    static readonly int[] best = { 0,
        1, 3, 3, 3, 3,
        3, 2, 3, 3, 2,   //10
        3, 2, 3, 4, 2,
        3, 3, 3, 4, 3,   //20
        3, 3, 4, 3, 4,
        3, 4, 2, 4, 3,   //30
        4, 3, 4, 4, 3,
        2, 4, 3, 4, 3,   //40
        4, 3, 2, 2, 2,
        3, 4, 3, 4, 3,   //50
        4, 2, 5, 4, 3,
        4, 5, 3, 5, 2,   //60
        4, 3, 3, 3, 3,
        4, 5, 3, 5, 3,   //70
        5, 3, 5, 4, 5,
        4, 5, 3, 5, 3,   //80
        3, 4, 5, 3, 5,
        4, 5, 1, 5, 4,   //90
        4, 3, 6, 5, 4,
        3, 4, 4, 4, 4    //100
    };
}
