using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class LevelGrid : MonoBehaviour {
    [SerializeField] private TextAsset _levelData;
    [SerializeField] private Pixel _pixelPrefab;
    private Grid _levelGrid;
    private List<Pixel> _levelPixels;

    private void Awake() {
        _levelGrid = GetComponent<Grid>();
        StartLevel(_levelData);
    }

    private void StartLevel(TextAsset levelData) {
        var dataJSON = JsonConvert.DeserializeObject<PixelImageJSON>(levelData.text);
        var dataIndex = 0;
        for (int y = 0; y < dataJSON.height; y++) {
            for (int x = 0; x < dataJSON.width; x++) {
                var localPos = new Vector3(x * (_levelGrid.cellSize.x + _levelGrid.cellGap.x), y *(_levelGrid.cellSize.y + _levelGrid.cellGap.y), 0);
                var pixel = Instantiate(_pixelPrefab, transform);
                pixel.transform.localPosition = localPos;
                Color newColor;
                if (ColorUtility.TryParseHtmlString(dataJSON.colorData[dataIndex], out newColor)) {
                    pixel.SetColor(newColor);
                }

                dataIndex++;
            }
        }
    }
}
