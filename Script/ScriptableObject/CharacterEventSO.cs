using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Events/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> onEventRaised;

    public void eventRaised(Character character)
    {
        onEventRaised?.Invoke(character);
    }
    
}
