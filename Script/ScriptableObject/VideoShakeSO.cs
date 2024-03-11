using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/VideoShakeSO")]
public class VideoShakeSO : ScriptableObject
{
    public UnityAction onEventRaised;

    public void eventRaised()
    {
        onEventRaised?.Invoke();
    }

}

