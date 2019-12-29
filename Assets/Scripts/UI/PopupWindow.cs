using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct Message {
    public string MessageID { get; }
    public string MessageTitle { get; }
    public string MessageBody { get; }
    public string ConfirmButtonText { get; }
    public string DeclineButtonText { get; }
    public Action OnPopupConfirmAction { get; private set; }
    public Action OnPopupDeclineAction { get; }

    public Message(string messageTitle, string messageBody, string confirmButtonText = "OK", Action onPopupConfirmAction = null, string declineButtonText = "Cancel", Action onPopupDeclineAction = null, string messageID = "") {
        MessageID = messageID;
        MessageTitle = messageTitle;
        MessageBody = messageBody;
        ConfirmButtonText = confirmButtonText;
        OnPopupConfirmAction = onPopupConfirmAction;
        DeclineButtonText = declineButtonText;
        OnPopupDeclineAction = onPopupDeclineAction;
    }

    public void AddConfirmationAction(Action action) {
        OnPopupConfirmAction += action;
    }
}

public abstract class PopupWindow<T> : WindowBehaviour<T> where T : WindowBehaviour<T> {
    [SerializeField] private TextMeshProUGUI _popupTitleText;
    [SerializeField] private TextMeshProUGUI _popupBodyText;
    [SerializeField] protected ButtonWithText _confirmButton;

    private Action _onPopupConfirmAction;
    private Queue<Message> _messagesQueue = new Queue<Message>();

    public void Construct(TextMeshProUGUI popupTitleText, TextMeshProUGUI popupBodyText, ButtonWithText confirmButton, float animTime) {
        _popupTitleText = popupTitleText;
        _popupBodyText = popupBodyText;
        _confirmButton = confirmButton;
        this.animTime = animTime;
        _messagesQueue.Clear();
        CloseWindow();
        RegisterButtons();
    }

    protected virtual void Start() {
        RegisterButtons();
    }

    protected virtual void RegisterButtons() {
        if (_confirmButton) {
            _confirmButton.OnClickActionWithValidation += ConfirmAndCloseWindow;
        }
    }

    public virtual void ShowPopup(Message message) {
        if (IsOpen) {
            _messagesQueue.Enqueue(message);
            return;
        }
        transform.SetAsLastSibling();
        RegisterActions(message);
        SetButtonsText(message);
        if (_popupTitleText != null) {
            _popupTitleText.text = message.MessageTitle;
        }
        if (_popupBodyText != null) {
            _popupBodyText.text = message.MessageBody;
        }
        base.ShowWindow();
    }

    protected virtual void RegisterActions(Message message) {
        _onPopupConfirmAction = message.OnPopupConfirmAction;
    }

    protected virtual void SetButtonsText(Message message) {
        _confirmButton?.SetButtonText(message.ConfirmButtonText);
    }

    protected virtual bool ConfirmAndCloseWindow() {
        _onPopupConfirmAction?.Invoke();
        _onPopupConfirmAction = null;
        CloseWindow();
        return true;
    }

    public override void CloseWindow() {
        base.CloseWindow();
        if (_messagesQueue.Count > 0) {
            ShowPopup(_messagesQueue.Dequeue());
        }
    }
}
