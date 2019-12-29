using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionsExtensions {

    public static T RemoveAndGetItem<T>(this IList<T> list, int indexToRemove) {
        var item = list[indexToRemove];
        list.RemoveAt(indexToRemove);
        return item;
    }

    public static List<T> RemoveAndGetItems<T>(this List<T> list, int count) {
        var items = list.GetRange(0, count);
        list.RemoveRange(0, items.Count);
        return items;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list) {
        return list == null || list.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this T[] array) {
        return array == null || array.Length == 0;
    }

    public static int IndexOf<T>(this T[] array, Predicate<T> predicate) {
        return Array.FindIndex(array, predicate);
    }

    public static void UpdateItem<T>(this List<T> list, T item, T newItem) {
        var itemIndex = list.IndexOf(item);
        if (itemIndex != -1) {
            list[itemIndex] = newItem;
        }
    }

    public static int SumArrayValues(this int[] array, int stopAtIndex) {
        stopAtIndex = Mathf.Min(array.Length, stopAtIndex);
        var sum = 0;
        for (int i = 0; i <= stopAtIndex; i++) {
            sum += array[i];
        }
        return sum;
    }

    public static T GetNextItem<T>(this List<T> list, T prevItem) {
        var nextItemIndex = list.IndexOf(prevItem) + 1;
        if (nextItemIndex < list.Count) {
            return list[nextItemIndex];
        }
        return default;
    }

    public static void Shuffle<T>(this List<T> list) {
        var n = list.Count;
        var rng = new System.Random(Guid.NewGuid().GetHashCode());
        while (n > 1) {
            n--;
            var k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount) {
        return list.OrderBy(_ => Guid.NewGuid()).Take(elementsCount).ToList();
    }
}
