using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("��������")]
    public float chaseSpeed;
    public float normalSpeed;
    public float currentSpeed;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    //ײǽ���
    [HideInInspector] public PhysicsCheck physicscheck;

    public Vector3 faceDir;

    //ײǽ֮��ȴ�һ���ٽ���ת��
    [Header("�ȴ�ʱ��")]
    public bool isWait;
    public float waitTime;
    public float currentWaitTime;
    [Header("��ʧʱ��")]
    public float lostTime;
    public float lostTimeCount;

    [Header("������")]
    public Transform attacker;

    public bool isHurt;
    public float hurtForce;

    public bool isDead;

    //������
    [Header("������")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    //����״̬��
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
        //������ʱ����enter����
        currState = patrolState;
        currState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        //��update�������߼�����
        currState.LogicUpdata();

    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !isWait)
            Move();

        //�ȴ�ʱ��
        TimeCount();

        currState.PhysicsUpdata();

    }

    private void OnDisable()
    {
        currState.OnExit();
    }

    //�����麯��ʹ����������޸�Move����
    public virtual void Move()
    {
        if (!isWait)
        {
            //����һ���ٶ������ƶ�
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }
    }

    private void TimeCount()
    {
        //����ײǽ�ȴ�ʱ���ʱ
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

        //��ʧ��Ҽ�ʱ
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

    //�л�״̬
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


    #region//����״̬�¼�
    //���˱�����
    public void TakeOnDamage(Transform attackerTrans)
    {
        attacker = attackerTrans;

        //�Ӻ󷽹�������ת��
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //���˱�����
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x,0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);

        //�����֮���ڶ�������ʱ��isHurtֵ��Ϊfalse
        //rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //������ʹ��Э��
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
