using UnityEngine;

public class Memory {

	public static int GetCurrentLevel() {
        return PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    public static void SetCurrentLevel(int value) {
        PlayerPrefs.SetInt("CurrentLevel", value);
    }

    public static bool[] GetStars() {
        return PlayerPrefsX.GetBoolArray("StarArray", false, 101);
    }

    public static void SetStar(int value) {
        bool[] stars = PlayerPrefsX.GetBoolArray("StarArray", false, 101);
        stars[value] = true;
        PlayerPrefsX.SetBoolArray("StarArray", stars);
    }

    public static int GetPlayLevel() {
        return PlayerPrefs.GetInt("PlayLevel", 1);
    }

    public static void SetPlayLevel(int value) {
        PlayerPrefs.SetInt("PlayLevel", value);
    }

    public static string GetFacebookID() {
        return PlayerPrefs.GetString("FacebookID");
    }

    public static void SetFacebookID(string id) {
        PlayerPrefs.SetString("FacebookID", id);
    }

    public static string GetFacebookName() {
        return PlayerPrefs.GetString("FacebookName");
    }

    public static void SetFacebookName(string name) {
        PlayerPrefs.SetString("FacebookName", name);
    }

    public static int GetScore() {
        if(System.DateTime.Today.Day != 1) {
            SetResetScore(true);
            return PlayerPrefs.GetInt("Score", 0);
        }
        else {
            if (GetResetScore()) {
                PlayerPrefs.SetInt("Score", 0);
                SetResetScore(false);
                return 0;
            }
            else {
                return PlayerPrefs.GetInt("Score", 0);
            }
        }        
    }

    public static void SetScore(int score) {
        int currScore = GetScore();
        PlayerPrefs.SetInt("Score", Mathf.Max(score, currScore));
    }

    private static bool GetResetScore() {
        return PlayerPrefsX.GetBool("ResetScore");
    }

    private static void SetResetScore(bool value) {
        PlayerPrefsX.SetBool("ResetScore", value);
    }
}
