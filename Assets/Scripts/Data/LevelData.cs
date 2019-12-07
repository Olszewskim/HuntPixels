using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelData {
    [ShowInInspector] public Vector2Int ImageDimensions { get; }
    [ShowInInspector] public Color[,] ImageColorsData { get; }
    [ShowInInspector] public List<ColorTask> LevelColorsTasks { get; } = new List<ColorTask>();

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
            LevelColorsTasks.Add(new ColorTask(task.Key, task.Value));
        }
    }
}
