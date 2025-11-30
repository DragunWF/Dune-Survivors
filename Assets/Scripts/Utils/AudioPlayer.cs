using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip upgradeClip;
    [SerializeField] private AudioClip errorClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }

    #region Audio Playback

    public void PlayPlayerShootClip()
    {
        if (playerShootClip != null)
        {
            PlayClip(playerShootClip, 0.7f);
        }
    }

    public void PlayDamageClip()
    {
        if (damageClip != null)
        {
            PlayClip(damageClip, 0.95f);
        }
    }

    public void PlayClickClip()
    {
        if (clickClip != null)
        {
            PlayClip(clickClip, 4.5f);
        }
    }

    public void PlayUpgradeClip()
    {
        if (upgradeClip != null)
        {
            PlayClip(upgradeClip, 2.25f);
        }
    }

    public void PlayErrorClip()
    {
        if (errorClip != null)
        {
            PlayClip(errorClip, 2.5f);
        }
    }

    #endregion

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
