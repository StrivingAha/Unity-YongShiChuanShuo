using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO LoadEventSO;
    public GameSceneSO sceneToGo;
    //´«ËÍÎ»ÖÃ
    public Vector3 positionToGo;


    public void OnTriggerAction()
    {
        gameObject.tag = "Untagged";

        LoadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
}
