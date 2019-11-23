using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Grid))]
public class LevelGrid : MonoBehaviour {
    [SerializeField] private TextAsset[] _levelsData;
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Button _nextLevelButton;
    private Grid _levelGrid;
    private List<Pixel> _levelPixels = new List<Pixel>();
    private int _currentLevelIndex;

    private void Start() {
        _levelGrid = GetComponent<Grid>();
        _nextLevelButton.onClick.AddListener(NextLevel);
        StartLevel(_levelsData[_currentLevelIndex]);
    }

    private void NextLevel() {
        _currentLevelIndex++;
        if (_currentLevelIndex == _levelsData.Length) {
            _currentLevelIndex = 0;
        }

        ClearOldLevel();
        StartLevel(_levelsData[_currentLevelIndex]);
    }



    private void StartLevel(TextAsset levelData) {
        var dataJSON = JsonConvert.DeserializeObject<PixelImageJSON>(levelData.text);
        var dataIndex = 0;
        var gridSize =
            new Vector3(dataJSON.width * _levelGrid.cellSize.x + (dataJSON.width - 1) * _levelGrid.cellGap.x,
                dataJSON.height * _levelGrid.cellSize.y + (dataJSON.height - 1) * _levelGrid.cellGap.y);
        for (int y = 0; y < dataJSON.height; y++) {
            for (int x = 0; x < dataJSON.width; x++) {
                var xPos = x * (_levelGrid.cellSize.x + _levelGrid.cellGap.x);
                var yPos = y * (_levelGrid.cellSize.y + _levelGrid.cellGap.y);
                var localPos = new Vector3(xPos, yPos, 0) - gridSize / 2 + _levelGrid.cellSize / 2;
                var pixel = Instantiate(_pixelPrefab, transform);
                pixel.transform.localPosition = localPos;
                Color newColor;
                if (ColorUtility.TryParseHtmlString(dataJSON.colorData[dataIndex], out newColor)) {
                    pixel.SetColor(newColor);
                }

                dataIndex++;
                _levelPixels.Add(pixel);
            }
        }

        _cameraController.FitCameraToGrid(gridSize.x, transform);
    }

    private void ClearOldLevel() {
        for (int i = 0; i < _levelPixels.Count; i++) {
            Destroy(_levelPixels[i].gameObject);
        }
        _levelPixels.Clear();
    }
}
