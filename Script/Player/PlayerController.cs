using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VideoShakeSO afterSceneLoadEvent;
    public VideoShakeSO LoadDataEvent;
    public VideoShakeSO backToMenuEvent;

    //地面检测
    //public GameObject physicsCheckObj;
    public PhysicsCheck physicsCheck;

    //inputSystem
    public PlayerInputControl inputControl;

    //上下左右方向
    public Vector2 inputDirection;
    private Rigidbody2D rb;

    public float speed=10;
    //将重力的scale设置为4
    public float jumpForce = 16.5f;

    //受伤弹回距离
    public bool isHurt;
    public float hurtForce = 8;

    //玩家死亡
    public bool isDead;

    //获取PlayerAnimation对象
    public PlayerAnimation playeranimation;
    public bool isAttack;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();

        //直接拿取其孩子，避免手动操作
        Transform physicsCheckTran = transform.GetChild(0);
        GameObject physicsCheckObj = physicsCheckTran.gameObject;
        physicsCheck = physicsCheckObj.GetComponent<PhysicsCheck>();

        //添加跳跃事件
        inputControl.gamePlay.Jump.started += Jump;

        //添加攻击事件
        inputControl.gamePlay.Attack.started += playerAttack;

        //获取PlayerAnimation对象
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
        //受伤以及攻击时不能移动
        if(!isHurt)
        {
            Move();
        }
    }

    //重新开始，玩家状态要改为活着
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    //加载场景时不可以移动
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

        //人物朝向
        if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1,1,1);
        else if(inputDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        //给予一个瞬时的力
        if(physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void playerAttack(InputAction.CallbackContext obj)
    {
        //感觉此处的触发器有点多余
        playeranimation.playerAttack1();
        isAttack = true;
    }

    #region unityEvent
    public void getHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        //使其反方向后退
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    //人物死亡取消控制器控制
    public void playerDie()
    {
        isDead = true;
        //只禁用游玩的操作可以执行ui层
        inputControl.gamePlay.Disable();
    }
    #endregion
}
