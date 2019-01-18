using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent (typeof (Text))]
public class TextResizer : MonoBehaviour {

    private Text phrase;

	void Start () {
        phrase = GetComponent<Text>();
        phrase.fontSize = (phrase.fontSize * Screen.width) / 600;
	}
	
}

//sizeOn600width - 600
// new size      - Screen.width