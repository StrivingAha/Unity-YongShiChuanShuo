using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("基本参数")]
    public float chaseSpeed;
    public float normalSpeed;
    public float currentSpeed;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    //撞墙检测
    [HideInInspector] public PhysicsCheck physicscheck;

    public Vector3 faceDir;

    //撞墙之后等待一会再进行转身
    [Header("等待时间")]
    public bool isWait;
    public float waitTime;
    public float currentWaitTime;
    [Header("丢失时间")]
    public float lostTime;
    public float lostTimeCount;

    [Header("攻击者")]
    public Transform attacker;

    public bool isHurt;
    public float hurtForce;

    public bool isDead;

    //检测玩家
    [Header("检测玩家")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    //有限状态机
    private BaseState currState;
    protected BaseState patrolState;
    protected BaseState chaseState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Transform child0 = transform.GetChild(1);
        physicscheck = child0.gameObject.GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        currentWaitTime = waitTime;
    }

    private void OnEnable()
    {
        //在启用时运行enter函数
        currState = patrolState;
        currState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        //在update中运行逻辑函数
        currState.LogicUpdata();

    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !isWait)
            Move();

        //等待时间
        TimeCount();

        currState.PhysicsUpdata();

    }

    private void OnDisable()
    {
        currState.OnExit();
    }

    //利用虚函数使得子类可以修改Move函数
    public virtual void Move()
    {
        if (!isWait)
        {
            //给予一个速度让其移动
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }
    }

    private void TimeCount()
    {
        //悬崖撞墙等待时间计时
        if (isWait)
        {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0)
            {
                transform.localScale = new Vector3(faceDir.x, 1, 1);
                isWait = false;
                currentWaitTime = waitTime;
            }
        }

        //丢失玩家计时
        if(!FoundPlayer())
        {
            lostTimeCount -=Time.deltaTime;
        }
        else
        {
            lostTimeCount = lostTime;
        }
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset, 0.2f);
    }

    //切换状态
    public void switchState(NPCStates state)
    {
        var newState = state switch
        {
            NPCStates.Patrol=>patrolState,
            NPCStates.Chase=>chaseState,
            _=>null
        };

        currState.OnExit();
        currState = newState;
        currState.OnEnter(this);

    }


    #region//敌人状态事件
    //敌人被攻击
    public void TakeOnDamage(Transform attackerTrans)
    {
        attacker = attackerTrans;

        //从后方攻击立即转身
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x,0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);

        //添加完之后在动画结束时将isHurt值改为false
        //rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //还可以使用协程
        StartCoroutine(onHurt(dir));

    }
    IEnumerator onHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt=false;
    }

    public void OnDead()
    {
        anim.SetBool("dead", true);
        isDead=true;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
