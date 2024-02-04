using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip pressClip;

    public void Hover()
    {
        audioSource.PlayOneShot(hoverClip);
    }

    public void Click()
    {
        audioSource.PlayOneShot(pressClip);
    }
}
