using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public AudioEventSO playerAudioEvent;
    public AudioClip audioClip;
    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAudioClip();
        
    }
    public void PlayAudioClip()
    {
        playerAudioEvent.OnEventRaised(audioClip);
    }
}
