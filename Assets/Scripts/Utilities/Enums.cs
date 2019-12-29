using System;
using UnityEngine.UI;

public static class Enums {
    public enum ColorNames {
        white,
        yellow,
        red,
        green
    }

    public static string GetEnumName<T>(T element) {
        return Enum.GetName(typeof(T), element);
    }

    public static void InitButtonsWithEnumType<T>(Button[] buttons, Action<T> buttonAction)
        where T : struct, IConvertible {
        if (buttons == null) {
            return;
        }

        if (!typeof(T).IsEnum) {
            throw new ArgumentException("T must be an enumerated type");
        }

        for (int i = 0; i < buttons.Length; i++) {
            if (Enum.IsDefined(typeof(T), i)) {
                buttons[i]?.onClick.RemoveAllListeners();
                T type = (T) (object) i;
                buttons[i]?.onClick.AddListener(() => buttonAction(type));
            } else {
                throw new ArgumentException(
                    $"InitButtonsWithEnumType -- There are more buttons to register than types in {typeof(T)}.");
            }
        }
    }
}
