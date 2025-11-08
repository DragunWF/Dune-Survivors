using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class FlashEffect : MonoBehaviour
{
    [Tooltip("Flash Type")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float effectDuration = 1.5f;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;

    private Coroutine flashRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalMaterial = spriteRenderer.material;
    }

    public void Flash()
    {
        // Stop any previous flash to avoid multiple coroutines running.
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        CancelInvoke(nameof(StopFlash));

        flashRoutine = StartCoroutine(StartFlashEffect());
        Invoke(nameof(StopFlash), effectDuration);
    }

    public void StopFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
        spriteRenderer.material = originalMaterial;
        CancelInvoke(nameof(StopFlash));
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
