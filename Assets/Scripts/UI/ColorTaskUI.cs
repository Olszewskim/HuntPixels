
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorTaskUI : MonoBehaviour {
    [SerializeField] private Image _colorToCollectImage;
    [SerializeField] private TextMeshProUGUI _requiredAmountText;

    public void Init(ColorTask colorTask) {
        _colorToCollectImage.color = colorTask.ColorToCollect;
        _requiredAmountText.text = $"x {colorTask.RequiredAmount}";
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
