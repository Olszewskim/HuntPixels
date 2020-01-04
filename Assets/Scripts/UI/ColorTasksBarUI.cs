using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorTasksBarUI : MonoBehaviour {
    [SerializeField] private ColorTaskUI _colorTaskUIPrefab;

    private readonly List<ColorTaskUI> _colorTaskUIs = new List<ColorTaskUI>();

    private void Awake() {
        _colorTaskUIPrefab.Hide();
        GameManager.OnLevelStarted += Init;
    }

    public void Init(LevelData currentLevel) {
        TurnOffElements();
        for (int i = 0; i < currentLevel.LevelColorsTasks.Count; i++) {
            if (i >= _colorTaskUIs.Count) {
                var colorTaskUI = Instantiate(_colorTaskUIPrefab, transform);
                _colorTaskUIs.Add(colorTaskUI);
            }

            _colorTaskUIs[i].Init( currentLevel.LevelColorsTasks[i]);
        }
    }

    private void TurnOffElements() {
        for (int i = 0; i < _colorTaskUIs.Count; i++) {
            _colorTaskUIs[i].Hide();
        }
    }

    private void OnDestroy() {
        GameManager.OnLevelStarted -= Init;
    }
}
