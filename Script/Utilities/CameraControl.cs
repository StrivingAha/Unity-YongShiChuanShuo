using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public VideoShakeSO afterSceneLoadedSO;

    private CinemachineConfiner confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VideoShakeSO cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner>();
    }

    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}

    private void OnEnable()
    {
        cameraShakeEvent.onEventRaised += OnCameraShakeEvent;
        afterSceneLoadedSO.onEventRaised += OnAfterSceneLoadedEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.onEventRaised -= OnCameraShakeEvent;
        afterSceneLoadedSO.onEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindWithTag("Bounds");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidatePathCache();
    }
}
