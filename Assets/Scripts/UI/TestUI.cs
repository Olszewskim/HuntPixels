using System;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour {
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _switchImageColorsButton;

    private void Start() {
        _nextLevelButton.onClick.AddListener(NextLevel);
        _switchImageColorsButton.onClick.AddListener(SwitchImageColors);
    }

    private void NextLevel() {
       GameManager.Instance.NextLevel();
    }

    private void SwitchImageColors() {
        GameManager.Instance.SwitchImageColors();
    }
}
