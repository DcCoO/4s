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
}
