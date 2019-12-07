using System.Collections.Generic;
using UnityEngine;

public class ColorTasksBarUI : MonoBehaviour {
    [SerializeField] private ColorTaskUI _colorTaskUIPrefab;

    private readonly List<ColorTaskUI> _colorTaskUIs = new List<ColorTaskUI>();

    private void Awake() {
        _colorTaskUIPrefab.Hide();
    }

    public void Init(List<ColorTask> colorTasks) {
        TurnOffElements();
        for (int i = 0; i < colorTasks.Count; i++) {
            if (i >= _colorTaskUIs.Count) {
                var colorTaskUI = Instantiate(_colorTaskUIPrefab, transform);
                _colorTaskUIs.Add(colorTaskUI);
            }

            _colorTaskUIs[i].Init(colorTasks[i]);
        }
    }

    private void TurnOffElements() {
        for (int i = 0; i < _colorTaskUIs.Count; i++) {
            _colorTaskUIs[i].Hide();
        }
    }
}
