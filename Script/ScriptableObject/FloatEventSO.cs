using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/FloatEventSO")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> onEventRaised;

    public void eventRaised(float amount)
    {
        onEventRaised?.Invoke(amount);
    }
}
