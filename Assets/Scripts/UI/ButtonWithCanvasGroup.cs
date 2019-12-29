using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Button))]
public class ButtonWithCanvasGroup : MonoBehaviour {
    private CanvasGroup _canvasGroup;
    private Button _button;
    private Func<bool> _buttonShouldBeActive;

    public Button Button {
        get {
            if (!_button) {
                _button = GetComponent<Button>();
            }
            return _button;
        }
    }

    public void RegisterButtonActivityFunc(Func<bool> func) {
        _buttonShouldBeActive += func;
    }

    public void RefreshButtonState(bool state) {
        state = state && (_buttonShouldBeActive?.Invoke() ?? true);
        if (!_canvasGroup) {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        if (_canvasGroup) {
            _canvasGroup.interactable = state;
            var alpha = state ? Constants.ENABLED_GROUP_ALPHA : Constants.DISABLED_GROUP_ALPHA;
            _canvasGroup.SetAlpha(alpha);
        }
    }
}
