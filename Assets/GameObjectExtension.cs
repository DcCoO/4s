using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension {

    public static RectTransform rectTransform(this GameObject go) {
        return go.GetComponent<RectTransform>();
    }
}
