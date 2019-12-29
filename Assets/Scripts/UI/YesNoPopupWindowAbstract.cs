using System;
using TMPro;
using UnityEngine;

public abstract class YesNoPopupWindowAbstract<T> : PopupWindow<T> where T : PopupWindow<T> {
    [SerializeField] private ButtonWithText _declineButton;

    private Action _onPopupDeclineAction;

    public void Construct(TextMeshProUGUI popupTitleText, TextMeshProUGUI popupBodyText, ButtonWithText confirmButton, ButtonWithText declineButton, float animTime) {
        _declineButton = declineButton;
        Construct(popupTitleText, popupBodyText, confirmButton, animTime);
    }

    protected override void RegisterButtons() {
        base.RegisterButtons();
        if (_declineButton != null)
            _declineButton.OnClickAction += DeclineAndCloseWindow;
    }

    protected override void RegisterActions(Message message) {
        base.RegisterActions(message);
        _onPopupDeclineAction = message.OnPopupDeclineAction;
    }

    protected override void SetButtonsText(Message message) {
        base.SetButtonsText(message);
        _declineButton.SetButtonText(message.DeclineButtonText);
    }

    protected virtual void DeclineAndCloseWindow() {
        _onPopupDeclineAction?.Invoke();
        _onPopupDeclineAction = null;
        CloseWindow();
    }
}
