using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    //ʹ��override���޸ĸ������
    //public override void Move()
    //{
    //    base.Move();
    //    if (!isWait)
    //    {
    //        anim.SetBool("walk", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("walk", false);
    //    }
    //}

    protected override void Awake()
    { 
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
}
