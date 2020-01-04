using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelData {
    [ShowInInspector] public Vector2Int ImageDimensions { get; }
    [ShowInInspector] public Color[,] ImageColorsData { get; }
    [TableList, ShowInInspector] public List<ColorTask> LevelColorsTasks { get; } = new List<ColorTask>();

    private Dictionary<Color, ColorTask> _levelColorTasksDictionary = new Dictionary<Color, ColorTask>();

    public LevelData(PixelImageJSON levelImageData) {
        ImageDimensions = new Vector2Int(levelImageData.width, levelImageData.height);
        ImageColorsData = new Color[ImageDimensions.x, ImageDimensions.y];
        var dataIndex = 0;

        for (int y = 0; y < ImageDimensions.y; y++) {
            for (int x = 0; x < ImageDimensions.x; x++) {
                Color newColor;
                if (ColorUtility.TryParseHtmlString(levelImageData.colorData[dataIndex], out newColor)) {
                    ImageColorsData[x, y] = newColor;
                }

                dataIndex++;
            }
        }

        CreateColorTasks();
    }

    private void CreateColorTasks() {
        var colorsToCollect =
            ImageColorsData.Cast<Color>().GroupBy(c => c).ToDictionary(cd => cd.Key, cd => cd.Count());
        foreach (var task in colorsToCollect) {
            var colorTask = new ColorTask(task.Key, task.Value);
            LevelColorsTasks.Add(colorTask);
            _levelColorTasksDictionary.Add(colorTask.ColorToCollect, colorTask);
        }
    }

    public void CollectPixel(GamePixel gamePixel) {
        if (_levelColorTasksDictionary.ContainsKey(gamePixel.myColor)) {
            _levelColorTasksDictionary[gamePixel.myColor].CollectColor();
        }
    }

    public Color GetRandomColorToCollect() {
        return LevelColorsTasks.GetRandomElement().ColorToCollect;
    }
}
