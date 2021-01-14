using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinemachine2DOneShotAudio : MonoBehaviour
{
    public AudioSource ForcePlay2DClip(AudioClip audioClip, Vector3 audioClipPosition)
    {
        GameObject oneShot2DAudio = new GameObject("One Shot 2D Audio");
        oneShot2DAudio.transform.position = audioClipPosition;
        AudioSource audio_source = oneShot2DAudio.AddComponent<AudioSource>();
        audio_source.spatialBlend = 0;
        audio_source.clip = audioClip;
        audio_source.Play();
        Destroy(oneShot2DAudio, audioClip.length);
        return audio_source;
    }
}
