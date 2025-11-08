using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashEffect : MonoBehaviour
{
    [Tooltip("Flash Type")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private bool isUsingPlayer;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;

    private Coroutine flashRoutine;
    private float effectDuration;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        effectDuration = 1.5f;
        originalMaterial = spriteRenderer.material;
    }

    public void Flash()
    {
        flashRoutine = StartCoroutine(StartFlashEffect());
        Invoke(nameof(StopFlash), effectDuration);
    }

    private void StopFlash()
    {
        StopCoroutine(flashRoutine);
        spriteRenderer.material = originalMaterial;
    }

    private IEnumerator StartFlashEffect()
    {
        var flashDuration = 0.25f;

        while (true)
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.material = originalMaterial;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
