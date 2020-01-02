using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class LevelGrid : SerializedMonoBehaviour {
    [SerializeField] private ImagePixel imagePixelPrefab;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameViewGrid _gameViewGrid;
    [SerializeField] private ColorTasksBarUI _colorTasksBarUI;

    private Grid _levelGrid;
    [ShowInInspector] [ReadOnly] private LevelData _currentLevel;
    private readonly List<ImagePixel> _levelPixels = new List<ImagePixel>();

    private float _cameraWidthCoverage = 0.6f;
    private float _gapPercentage = 0.05f;
    private float _percentageGridPosFromTopEdge = 0.03f;

    private void Awake() {
        _levelGrid = GetComponent<Grid>();
        GamePixel.OnGamePixelCollected += CollectGamePixel;
    }

    public void StartLevel(LevelData levelData) {
        ClearOldLevel();
        _currentLevel = levelData;
        CalculateGridSizes(_currentLevel.ImageDimensions.x);

        for (int y = 0; y < _currentLevel.ImageDimensions.y; y++) {
            for (int x = 0; x < _currentLevel.ImageDimensions.x; x++) {
                var xPos = x * (_levelGrid.cellSize.x + _levelGrid.cellGap.x);
                var yPos = y * (_levelGrid.cellSize.y + _levelGrid.cellGap.y);
                var localPos = new Vector3(xPos, yPos, 0) + _levelGrid.cellSize / 2;

                var pixel = Instantiate(imagePixelPrefab, transform);
                pixel.transform.localPosition = localPos;
                pixel.transform.localScale = _levelGrid.cellSize;
                pixel.SetColor(_currentLevel.ImageColorsData[x, y]);
                _levelPixels.Add(pixel);
            }
        }

        PlaceGridAtTopOfScreen(_currentLevel.ImageDimensions.x, _currentLevel.ImageDimensions.y);
        _gameViewGrid.InitLevel(_currentLevel.LevelColorsTasks);
        _colorTasksBarUI.Init(_currentLevel.LevelColorsTasks);
    }

    private void PlaceGridAtTopOfScreen(int dataWidth, int dataHeight) {
        var gridWidth = dataWidth * _levelGrid.cellSize.x + (dataWidth - 1) * _levelGrid.cellGap.x;
        var gridHeight = dataHeight * _levelGrid.cellSize.y + (dataHeight - 1) * _levelGrid.cellGap.y;
        var camHeight = _cameraController.GetCameraHeight();
        _levelGrid.transform.position = new Vector3(
            -gridWidth / 2,
            camHeight / 2 - gridHeight - _percentageGridPosFromTopEdge * camHeight,
            0
        );
    }

    private void CalculateGridSizes(float pictureElements) {
        var camWidth = _cameraController.GetCameraWidth() * _cameraWidthCoverage;
        var widthPerElement = camWidth / pictureElements;
        var gapWidth = widthPerElement * _gapPercentage;
        widthPerElement -= gapWidth;
        _levelGrid.cellSize = new Vector3(widthPerElement, widthPerElement, widthPerElement);
        _levelGrid.cellGap = new Vector3(gapWidth, gapWidth, gapWidth);
    }

    private void ClearOldLevel() {
        for (var i = 0; i < _levelPixels.Count; i++) {
            Destroy(_levelPixels[i].gameObject);
        }

        _levelPixels.Clear();
        _currentLevel = null;
    }

    public void SwitchImageColors() {
        for (int i = 0; i < _levelPixels.Count; i++) {
            _levelPixels[i].SwitchColor();
        }
    }

    private void CollectGamePixel(GamePixel gamePixel) {
        _currentLevel?.CollectPixel(gamePixel);
        var unfulfilledPixelsOfCollectedColor =
            _levelPixels.Where(p => p.myColor == gamePixel.myColor && !p.IsFulfilled).ToArray();
        if (unfulfilledPixelsOfCollectedColor.Length > 0) {
            var randomPixel = unfulfilledPixelsOfCollectedColor.GetRandomElement();
            randomPixel.FulfillImagePixel();
        }
    }

    private void OnDestroy() {
        GamePixel.OnGamePixelCollected -= CollectGamePixel;
    }
}
