using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    public int level = 1;
    public Text phrase;
    public Text levelNumber;
    public RectTransform dialog, undoButton, restartButton, hintButton, backButton, restartButton2, nextButton, levelPiece;
    public Image shadow, moveArea;
    public GameObject star;
    public Sprite rightBaloon;
    public Image dialogImage;

    bool gotStar = false;
    bool readyToNext = false;
    string[] star_phrase = {"Fantastic!","Amazing!","Incredible!","Brilliant!","Wonderful!","Awesome!", "Flawless!" };
    string[] win_phrase = { "Good!", "Nice!", "You did it!", "Very good!", "Great!", "Well done!", "Way to go!" };

    void Awake() {
        instance = this;
    }
    // Use this for initialization
    void Start() {
        level = Memory.GetPlayLevel();
        levelNumber.text = level.ToString();
        phrase.text = $"Can you turn all these pieces into {level}?";

    }

    // Update is called once per frame
    public void CheckResult() {
        if(PieceManager.instance.pieces.Count == 1) {
            if(PieceManager.instance.pieces[0].GetComponent<PieceBehaviour>().value == level) {
                ActionManager.instance.TurnButtons(false, false, false);
                StartCoroutine(EndGame());
            }
        }
    }

    void DoNothing(GameObject g) { }

    IEnumerator EndGame() {
        gotStar = ActionManager.instance.GetNumOp() == best[level];
        Memory.SetCurrentLevel(Mathf.Max(level + 1, Memory.GetCurrentLevel()));
        if (gotStar) Memory.SetStar(level);
        float time = 30;
        //desativa clique da peça, manda ela pro meio e some com os botoes, liga a sombra com alpha 0
        PieceBehaviour pb = PieceManager.instance.pieces[0].GetComponent<PieceBehaviour>();
        pb.touchCallback = pb.releaseCallback = DoNothing;
        //0.62
        Vector2 begin = pb.rt.anchoredPosition;
        Color c1 = shadow.color; Color c2 = moveArea.color;

        //move peça para o meio
        for (float i = 0; i <= time; i++) {
            pb.rt.anchoredPosition = Vector2.Lerp(begin, Vector2.zero, i / time);
            yield return null;
        }

        //some tudo e liga sombra
        for (float i = 0; i <= time; i++) {
            dialog.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            undoButton.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            restartButton.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            hintButton.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            backButton.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            c2.a = Mathf.Lerp(1, 0, i / time); moveArea.color = c2;
            c1.a = Mathf.Lerp(0, 0.7f, i / time); shadow.color = c1;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //explode peça e liga estrela se possivel
        star.SetActive(gotStar);
        PieceManager.instance.pieces[0].GetComponent<PieceBehaviour>().Explode();

        yield return new WaitForSeconds(0.5f);

        //cresce balao de fala very good ou amazing
        Random r = new Random();
        if (gotStar) {
            phrase.text = star_phrase[r.Next(0, star_phrase.Length)];
        }
        else {
            phrase.text = win_phrase[r.Next(0, win_phrase.Length)];
        }
        for (float i = 0; i <= time; i++) {
            dialog.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //diminui balao de fala
        for (float i = 0; i <= time; i++) {
            dialog.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //nasce balao com ponta na peça nova e nascem botoes        

        //arrow.SetActive(false);
        oldSprite = dialogImage.sprite;
        phrase.text = "Are you ready for the next challenge?";
        dialogImage.sprite = rightBaloon;

        if(level != 100) StartCoroutine(ExtraPiece.instance.Appear());

        ActionManager.instance.TurnButtons(false, true, true);

        for (float i = 0; i <= time; i++) {
            if (level != 100) nextButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            restartButton2.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            dialog.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            backButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            yield return null;
        }

        gotStar = false;
        readyToNext = true;
    }

    public void NextLevel() {
        if (readyToNext) {
            readyToNext = false;
            Memory.SetPlayLevel(++level);
            StartCoroutine(PrepareNextLevel());
        }
    }

    private Sprite oldSprite;
    IEnumerator PrepareNextLevel() {
        float time = 30;
        //some com balao velho, move peça nova, some botoes novos
        StartCoroutine(ExtraPiece.instance.Replace());
        Vector2 sp = levelPiece.anchoredPosition;
        Vector2 ep = new Vector2(- levelPiece.rect.width, sp.y);
        for (float i = 0; i <= time; i++) {
            nextButton.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            restartButton2.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            dialog.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            levelPiece.anchoredPosition = Vector2.Lerp(sp, ep, i / time);
            star.rectTransform().localScale = Vector2.Lerp(Vector2.one, Vector2.zero, i / time);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //some com sombra, volta botoes antigos, volta balao antigo, seta novo level, ativa ponta
        //arrow.SetActive(true);
        dialogImage.sprite = oldSprite;
        phrase.text = $"Can you turn all these pieces into {level}?";
        Color c1 = shadow.color; Color c2 = moveArea.color;
        for (float i = 0; i <= time; i++) {
            restartButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            hintButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            undoButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            dialog.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            c2.a = Mathf.Lerp(0, 1, i / time); moveArea.color = c2;
            c1.a = Mathf.Lerp(0.7f, 0, i / time); shadow.color = c1;
            yield return null;
        }

        //desativa a estrela
        levelPiece.anchoredPosition = sp;
        star.SetActive(false);
        star.rectTransform().localScale = Vector2.one;
        levelNumber.text = $"{level}";
        ExtraPiece.instance.MoveOut();
        ActionManager.instance.Restart();
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
        //some com balao velho, move peça nova, some botoes novos
        StartCoroutine(ExtraPiece.instance.Disappear());
        //Vector2 sp = levelPiece.anchoredPosition;
        //Vector2 ep = new Vector2(-levelPiece.rect.width, sp.y);
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
        phrase.text = $"Can you turn all these pieces into {level}?";
        Color c1 = shadow.color; Color c2 = moveArea.color;
        for (float i = 0; i <= time; i++) {
            restartButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            hintButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            undoButton.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            dialog.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i / time);
            c2.a = Mathf.Lerp(0, 1, i / time); moveArea.color = c2;
            c1.a = Mathf.Lerp(0.7f, 0, i / time); shadow.color = c1;
            yield return null;
        }

        //desativa a estrela
        //levelPiece.anchoredPosition = sp;
        star.SetActive(false);
        star.rectTransform().localScale = Vector2.one;
        //levelNumber.text = $"{level}";
        //ExtraPiece.instance.MoveOut();
        ActionManager.instance.Restart();
    }

    public void ReturnHome() {
        SceneManager.LoadScene("Levels");
    }

    //TODO texto de cada uma das fases

    public static int[] best = { 0,
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

    public static int[,] numbers = { {0, 0},
        {44, 44}, {1, 1}, {12, 4}, {4, 0}, {20, 4},
        {4, 2}, {11, 4}, {12, 4}, {8, 1}, {40, 4},
        {44, 4}, {48, 4}, {11, 2}, {12, 2}, {11, 4},
        {16, 1}, {16, 1}, {22, 4}, {20, 1}, {40, 2},
        {11, 10}, {44, 2}, {46, 2}, {16, 8}, {24, 1},
        {22, 4}, {24, 3}, {44, 16}, {24, 5}, {44, 14},
        {75, 44}, {16, 16}, {11, 3}, {44, 10}, {24, 11},
        {44, 8}, {40, 3}, {40, 2}, {40, 1}, {44, 4},
        {40, 1}, {44, 2}, {44, 1}, {44, 1}, {44, 1},
        {44, 2}, {44, 3}, {44, 2}, {44, 5}, {44, 6},
        {44, 7}, {44, 8}, {55, 2}, {44, 10}, {40, 4},
        {44, 12}, {44, 13}, {44, 14}, {55, 4}, {44, 16},
        {105, 44}, {66, 4}, {252, 4}, {44, 20}, {260, 4},
        {44, 22}, {75, 8}, {64, 4}, {276, 4}, {66, 4},
        {284, 4}, {44, 28}, {292, 4}, {444, 6}, {55, 20},
        {80, 4}, {308, 4}, {48, 4}, {316, 4}, {20, 4},
        {4, 3}, {820, 10}, {75, 8}, {44, 40}, {88, 3},
        {344, 4}, {55, 32}, {44, 44}, {45, 44}, {66, 24},
        {11, 2}, {88, 4}, {96, 3}, {96, 2}, {96, 1},
        {48, 2}, {96, 1}, {88, 10}, {55, 44}, {25, 4}
    };
}
