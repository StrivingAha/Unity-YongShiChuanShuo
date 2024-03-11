using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoadEventSO sceneLoadEvent;
    public VideoShakeSO afterSceneLoadEvent;
    public VideoShakeSO LoadDataEvent;
    public VideoShakeSO backToMenuEvent;

    //������
    //public GameObject physicsCheckObj;
    public PhysicsCheck physicsCheck;

    //inputSystem
    public PlayerInputControl inputControl;

    //�������ҷ���
    public Vector2 inputDirection;
    private Rigidbody2D rb;

    public float speed=10;
    //��������scale����Ϊ4
    public float jumpForce = 16.5f;

    //���˵��ؾ���
    public bool isHurt;
    public float hurtForce = 8;

    //�������
    public bool isDead;

    //��ȡPlayerAnimation����
    public PlayerAnimation playeranimation;
    public bool isAttack;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();

        //ֱ����ȡ�亢�ӣ������ֶ�����
        Transform physicsCheckTran = transform.GetChild(0);
        GameObject physicsCheckObj = physicsCheckTran.gameObject;
        physicsCheck = physicsCheckObj.GetComponent<PhysicsCheck>();

        //�����Ծ�¼�
        inputControl.gamePlay.Jump.started += Jump;

        //��ӹ����¼�
        inputControl.gamePlay.Attack.started += playerAttack;

        //��ȡPlayerAnimation����
        playeranimation = GetComponent<PlayerAnimation>();

        inputControl.Enable();
    }


    private void OnEnable()
    {  
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.onEventRaised += OnafterSceneLoadEvent;
        LoadDataEvent.onEventRaised += OnLoadDataEvent;
        backToMenuEvent.onEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.onEventRaised += OnafterSceneLoadEvent;
        LoadDataEvent.onEventRaised -= OnLoadDataEvent;
        backToMenuEvent.onEventRaised -= OnLoadDataEvent;
    }


    private void Update()
    {
        inputDirection = inputControl.gamePlay.Move.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        //�����Լ�����ʱ�����ƶ�
        if(!isHurt)
        {
            Move();
        }
    }

    //���¿�ʼ�����״̬Ҫ��Ϊ����
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    //���س���ʱ�������ƶ�
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.gamePlay.Disable();
    }

    private void OnafterSceneLoadEvent()
    {
        inputControl.gamePlay.Enable();
    }

    private void Move()
    {
        transform.position += new Vector3(inputDirection.x, 0, 0)*speed*Time.deltaTime;

        //���ﳯ��
        if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1,1,1);
        else if(inputDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        //����һ��˲ʱ����
        if(physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void playerAttack(InputAction.CallbackContext obj)
    {
        //�о��˴��Ĵ������е����
        playeranimation.playerAttack1();
        isAttack = true;
    }

    #region unityEvent
    public void getHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        //ʹ�䷴�������
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    //��������ȡ������������
    public void playerDie()
    {
        isDead = true;
        //ֻ��������Ĳ�������ִ��ui��
        inputControl.gamePlay.Disable();
    }
    #endregion
}
