using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtensions {

    public static void RegisterButton(this Button button, UnityAction unityAction) {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unityAction);
    }

    public static void OverrideSprite(this Button button, Sprite sprite) {
        button.image.overrideSprite = sprite;
    }

    public static void SetSprite(this Button button, Sprite sprite) {
        button.image.sprite = sprite;
    }
}
