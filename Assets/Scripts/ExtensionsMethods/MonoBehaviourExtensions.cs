using UnityEngine;

public static class MonoBehaviourExtensions {

    public static void SetActive(this MonoBehaviour obj, bool state) {
        obj.gameObject.SetActive(state);
    }

    public static bool CompareLayer(this Component obj, int layerToCompare) {
        return obj.gameObject.layer == layerToCompare;
    }
}
