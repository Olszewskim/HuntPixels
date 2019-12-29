using System;
using System.Globalization;
using UnityEngine;

public static class NumericExtensions {

    public static float ToTwoDecimalPlaces(this float number) {
        return Mathf.Round(number * 100f) / 100f;
    }


    public static string ToPercentString(this double number) {
        return ((float)number).ToPercentString();
    }

    public static string ToPercentString(this float number) {
        return $"{number * 100}%";
    }

    public static string ConvertToTimeStringAuto(this uint seconds) {
        return seconds < Constants.SECONDS_IN_HOUR ? seconds.ConvertToTimeStringMSFormat() : seconds.ConvertToTimeStringHMSFormat();
    }

    public static string ConvertToTimeStringMSFormat(this uint seconds) {
        var min = Mathf.FloorToInt(seconds / 60F);
        var sec = Mathf.FloorToInt(seconds - min * 60);
        return string.Format("{0:00}:{1:00}", min, sec);
    }

    public static string ConvertToTimeStringMSMSFormat(this float seconds) {
        var min = Mathf.FloorToInt(seconds / 60F);
        var sec = Mathf.FloorToInt(seconds - min * 60);
        var milliSec = Mathf.FloorToInt(seconds * 1000) % 1000;
        return string.Format("{0:00}:{1:00}:{2:000}", min, sec, milliSec);
    }

    public static string ConvertToTimeStringHMSFormat(this uint seconds) {
        int hours = Mathf.FloorToInt(seconds / 3600F);
        int min = Mathf.FloorToInt((seconds - hours * 3600) / 60F);
        int sec = Mathf.FloorToInt(seconds - hours * 3600 - min * 60);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, min, sec);
    }

    public static string ConvertToString(this float number) {
        return number.ToString("0.#", CultureInfo.InvariantCulture);
    }

    public static string ConvertToThousandsFormat<T>(this T number) where T : struct, IFormattable {
        var numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        numberFormatInfo.NumberGroupSeparator = " ";
        return number.ToString("#,0", numberFormatInfo);
    }
}
