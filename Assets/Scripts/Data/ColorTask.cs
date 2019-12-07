using Sirenix.OdinInspector;
using UnityEngine;

public class ColorTask {
    [ShowInInspector] public Color ColorToCollect { get; }
    [ShowInInspector] public int RequiredAmount { get; }
    public bool IsCompleted => _currentCollected >= RequiredAmount;

    [ShowInInspector]  private int _currentCollected;

    public ColorTask(Color colorToCollect, int requiredAmount) {
        ColorToCollect = colorToCollect;
        RequiredAmount = requiredAmount;
    }
}
