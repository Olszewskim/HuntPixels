using System;

public abstract class Screen<T> : Singleton<T> where T : Singleton<T> {

    public event Action OnScreenOpened;

    public event Action OnScreenClosed;

    public bool IsScreenActive => gameObject.activeSelf;

    public virtual void TurnOnScreen() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
            OnScreenOpened?.Invoke();
        }
    }

    public virtual void TurnOffScreen() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
            OnScreenClosed?.Invoke();
        }
    }

    public bool IsActive() {
        return gameObject.activeSelf;
    }

    protected virtual void OnBack() {
    }
}
