using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class DreamEffects : MonoBehaviour
{
    public PostProcessVolume dreamVolume;
    private ChromaticAberration dreamEffect;
    public float floor;
    public float scale;

    public SpriteRenderer spriteRenderer;
    public Sprite[] noiseSprites;
    private int currentSpriteIndex = 0;

    private float timer = 0;
    public float speed = 1f;

    private void Start()
    {
        if (dreamEffect != null)
            dreamVolume.profile.TryGetSettings<ChromaticAberration>(out dreamEffect);
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (dreamEffect != null)
            dreamEffect.intensity.value = floor + scale * Mathf.Sin(Time.time);

        timer += Time.deltaTime;

        if (timer > speed)
        {
            timer -= speed;
            NextNoise();
        }
    }

    private void NextNoise()
    {
        currentSpriteIndex += 1;
        if (currentSpriteIndex == 8)
        {
            currentSpriteIndex = 0;
        }
        spriteRenderer.sprite = noiseSprites[currentSpriteIndex];
    }
}
