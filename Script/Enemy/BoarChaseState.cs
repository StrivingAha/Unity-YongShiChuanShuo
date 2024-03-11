using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
     public override void OnEnter(Enemy enemy)
    {
        currEnemy = enemy;
        currEnemy.currentSpeed = currEnemy.chaseSpeed;
        currEnemy.anim.SetBool("run", true);
    }   
    
    public override void LogicUpdata()
    {
        if(currEnemy.lostTimeCount<=0)
            currEnemy.switchState(NPCStates.Patrol);

        //����ǽ�������µ�ת����
        if (currEnemy.physicscheck.touchWall || !currEnemy.physicscheck.isGround)
        {
            currEnemy.transform.localScale = new Vector3(currEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdata()
    {

    }


    public override void OnExit()
    {
        currEnemy.anim.SetBool("run", false);
    }


}
