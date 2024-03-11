using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Events/PlayerAudioEventSO")]
public class AudioEventSO : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;
    public void RaiseEvent(AudioClip audioClip)
    {
        OnEventRaised?.Invoke(audioClip);
    }
}
