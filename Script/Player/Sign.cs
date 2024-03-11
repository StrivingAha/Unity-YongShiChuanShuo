using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    public GameObject SignE;
    public Transform playerTran;
    private Animator animE;
    private bool canPress;

    private IInteractable targetItem;

    private PlayerInputControl playerInput;

    private void Awake()
    {
        animE = SignE.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        playerInput.gamePlay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void Update()
    {
        SignE.SetActive(canPress);
        SignE.transform.localScale = playerTran.localScale;
    }

    //调用接口的方法
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.OnTriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
            //交互一次后消失
            canPress = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = false;
        }
    }
}
