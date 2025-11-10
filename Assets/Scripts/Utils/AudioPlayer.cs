using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip upgradeClip;
    [SerializeField] private AudioClip errorClip;

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
            PlayClip(clickClip, 1f);
        }
    }

    public void PlayUpgradeClip()
    {
        if (upgradeClip != null)
        {
            PlayClip(upgradeClip, 1f);
        }
    }

    public void PlayErrorClip()
    {
        if (errorClip != null)
        {
            PlayClip(errorClip, 0.85f);
        }
    }

    #endregion

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector2 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
