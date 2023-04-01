using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayerOrderToNegY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public bool RenderLow;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100f);

        if (RenderLow)
        {
            spriteRenderer.sortingOrder = -999;
        }
    }
}
