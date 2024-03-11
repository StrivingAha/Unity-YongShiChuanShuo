using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerAnimation : MonoBehaviour
{
    //����һ���ű�ȡ��x������
    private PlayerController playerRun;

    //����һ���ű���ȡ�Ƿ��ڵ���
    private PhysicsCheck checkIsGround;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerRun = GetComponent<PlayerController>();

        Transform physicsCheckTran = transform.GetChild(0);
        GameObject physicsCheckObj = physicsCheckTran.gameObject;
        checkIsGround = physicsCheckObj.GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        setAnimation();
    }

    private void setAnimation()
    {
        //�����ܶ�
        anim.SetFloat("runTime",Mathf.Abs(playerRun.inputDirection.x));

        //������Ծ
        anim.SetFloat("JumpTime",rb.velocity.y);
        anim.SetBool("isGround", checkIsGround.isGround);

        //��������
        anim.SetBool("isDead",playerRun.isDead);

        //���﹥��
        anim.SetBool("isAttack",playerRun.isAttack);

    }

    //������˴�����˸
    public void playerHurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void playerAttack1()
    {
        anim.SetTrigger("attack");
    }

}
