using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip damageClip;

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
