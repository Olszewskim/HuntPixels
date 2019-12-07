using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelData {
    [ShowInInspector] public Vector2Int ImageDimensions { get; }
    [ShowInInspector] public Color[,] ImageColorsData { get; }
    [ShowInInspector] public HashSet<Color> LevelColors { get; } = new HashSet<Color>();

    public LevelData(PixelImageJSON levelImageData) {
        ImageDimensions = new Vector2Int(levelImageData.width, levelImageData.height);
        ImageColorsData = new Color[ImageDimensions.x, ImageDimensions.y];
        var dataIndex = 0;

        for (int y = 0; y < ImageDimensions.y; y++) {
            for (int x = 0; x < ImageDimensions.x; x++) {
                Color newColor;
                if (ColorUtility.TryParseHtmlString(levelImageData.colorData[dataIndex], out newColor)) {
                    ImageColorsData[x, y] = newColor;
                    LevelColors.Add(newColor);
                }

                dataIndex++;
            }
        }
    }
}
