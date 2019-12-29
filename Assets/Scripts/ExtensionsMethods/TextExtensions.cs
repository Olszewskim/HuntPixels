using TMPro;
using static Enums;

public static class TextExtensions {

    public static string ColorString(this string text, ColorNames colorName) {
        return $"<color=\"{GetEnumName(colorName)}\">{text}</color>";
    }

    public static bool IsNullOrEmpty(this string text) {
        return string.IsNullOrEmpty(text);
    }

    public static bool IsNullOrEmpty(this TextMeshProUGUI text) {
        return string.IsNullOrEmpty(text.text);
    }
}
