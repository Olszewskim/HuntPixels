using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ColorTask {

    public event Action OnTaskProgressWasMade;
    [ShowInInspector] [PropertyOrder(0)] public Color ColorToCollect { get; }

    [ShowInInspector] [PropertyOrder(2)] public int RequiredAmount { get; }

    [ShowInInspector] [PropertyOrder(3)] public bool IsCompleted => _currentCollected >= RequiredAmount;

    [ShowInInspector] [PropertyOrder(1)] private int _currentCollected;

    public ColorTask(Color colorToCollect, int requiredAmount) {
        ColorToCollect = colorToCollect;
        RequiredAmount = requiredAmount;
    }

    public void CollectColor() {
        if (!IsCompleted) {
            _currentCollected++;
            OnTaskProgressWasMade?.Invoke();
        }
    }

    public int GetAmountToCollect() {
        return Mathf.Max(RequiredAmount - _currentCollected, 0);
    }
}
