using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : MonoBehaviour {
    float initialHeight = 1f;
    float finalHeight = 3f;
    float animationDuration = 3f;

    private BoxCollider2D box;

    private void Start() {
        box = GetComponent<BoxCollider2D>();
    }

    IEnumerator SmoothLerpCollider(float startHeight, float endHeight, float duration) {
        float elapsed = 0f;

        Vector2 originalSize = box.size;
        Vector2 originalOffset = box.offset;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newHeight = Mathf.SmoothStep(startHeight, endHeight, t);

            box.size = new Vector2(originalSize.x, newHeight);

            box.offset = new Vector2(originalOffset.x, newHeight / 2f);

            yield return null;
        }

        box.size = new Vector2(originalSize.x, endHeight);
        box.offset = new Vector2(originalOffset.x, endHeight / 2f);
    }

    public void GrowSapling() {
        //Start animation
        StartCoroutine(SmoothLerpCollider(initialHeight, finalHeight, animationDuration));
    }
}
