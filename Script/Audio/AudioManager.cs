using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGMsource;
    public AudioSource currSource;

    [Header("ÊÂ¼þ¼àÌý")]
    public FloatEventSO volumeEvent;
    public AudioEventSO BGMAudioEvent;
    public AudioEventSO currAudioEvent;
    public VideoShakeSO pauseEvent;
    

    [Header("¹ã²¥")]
    public FloatEventSO SyncVolumeEvent;

    public AudioMixer Mixer;

    private void OnEnable()
    {
        currAudioEvent.OnEventRaised += OnCurrEvent;
        BGMAudioEvent.OnEventRaised += OnBGMEvent;
        volumeEvent.onEventRaised += OnvolumeEvent;
        pauseEvent.onEventRaised += onpauseEvent;
        
    }

    private void OnDisable()
    {
        currAudioEvent.OnEventRaised -= OnCurrEvent;
        BGMAudioEvent.OnEventRaised -= OnBGMEvent;
        volumeEvent.onEventRaised -= OnvolumeEvent;
        pauseEvent.onEventRaised -= onpauseEvent;
        
    }

    

    private void onpauseEvent()
    {
        float amount;
        Mixer.GetFloat("MasterVolume",out amount);
        SyncVolumeEvent.eventRaised(amount);
    }

    private void OnvolumeEvent(float amount)
    {
        Mixer.SetFloat("MasterVolume",amount*50-30);
    }

    private void OnCurrEvent(AudioClip Clip)
    {
        currSource.clip = Clip;
        currSource.Play();
    }

    private void OnBGMEvent(AudioClip Clip)
    {
        BGMsource.clip = Clip;
        BGMsource.Play();
    }
}
