using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("�㲥")]
    public VideoShakeSO saveDataEvent;

    public SpriteRenderer spriteRenderer;

    public Sprite darkImage;
    public Sprite lightImage;

    private bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightImage : darkImage;
    }

    public void OnTriggerAction()
    {
        if(!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightImage;

            //��������
            saveDataEvent.eventRaised();

            this.gameObject.tag = "Untagged";
        }
    }
}
