
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorTaskUI : MonoBehaviour {

    [SerializeField] private Image _colorToCollectImage;
    [SerializeField] private TextMeshProUGUI _requiredAmountText;

    private ColorTask _myTask;

    public void Init(ColorTask colorTask) {
        _myTask = colorTask;
        colorTask.OnTaskProgressWasMade += RefreshView;
        RefreshView();
        gameObject.SetActive(true);
    }

    private void RefreshView() {
        _colorToCollectImage.color = _myTask.ColorToCollect;
        _requiredAmountText.text = $"x {_myTask.GetAmountToCollect()}";
        if (_myTask.IsCompleted) {
            Hide();
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
