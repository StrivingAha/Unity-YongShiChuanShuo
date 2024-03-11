using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerAnimation : MonoBehaviour
{
    //从另一个脚本取得x轴数据
    private PlayerController playerRun;

    //从另一个脚本获取是否在地面
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
        //人物跑动
        anim.SetFloat("runTime",Mathf.Abs(playerRun.inputDirection.x));

        //人物跳跃
        anim.SetFloat("JumpTime",rb.velocity.y);
        anim.SetBool("isGround", checkIsGround.isGround);

        //人物死亡
        anim.SetBool("isDead",playerRun.isDead);

        //人物攻击
        anim.SetBool("isAttack",playerRun.isAttack);

    }

    //玩家受伤触发闪烁
    public void playerHurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void playerAttack1()
    {
        anim.SetTrigger("attack");
    }

}
