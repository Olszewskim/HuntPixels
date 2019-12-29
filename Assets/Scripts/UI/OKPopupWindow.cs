public class OKPopupWindow : PopupWindow<OKPopupWindow> {

    protected override void ForceCloseWindow() {
        ConfirmAndCloseWindow();
    }
}
