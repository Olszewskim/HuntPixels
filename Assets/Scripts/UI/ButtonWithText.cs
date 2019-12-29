using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonWithText : ButtonWithCanvasGroup {

    public event Action OnClickAction;

    public event Func<bool> OnClickActionWithValidation;

    [SerializeField] private TextMeshProUGUI _text;

    protected virtual void Awake() {
        RegisterButton();
    }

    private void RegisterButton() {
        Button.RegisterButton(() => OnClick());
    }

    public virtual bool? OnClick() {
        OnClickAction?.Invoke();
        return OnClickActionWithValidation?.Invoke();
    }

    public void SetButtonText(string txt) {
        if (_text) {
            _text.text = txt;
        }
    }

    public bool CompareTexts(string txt) {
        return _text.text == txt;
    }

    public void SetButtonTextVisibilityState(bool state) {
        if (_text) {
            _text.SetActive(state);
        }
    }

    public void ChangeInteractableState(bool state) {
        Button.interactable = state;
    }

    public bool IsInteractable() {
        return Button.interactable;
    }
}
