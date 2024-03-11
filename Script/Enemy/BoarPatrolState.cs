

using System.Diagnostics;

public class BoarPatrolState : BaseState
{


    public override void OnEnter(Enemy enemy)
    {
        currEnemy = enemy;
        currEnemy.currentSpeed = currEnemy.normalSpeed;
        currEnemy.anim.SetBool("walk", true);
    }

    public override void LogicUpdata()
    {
        //��������ǽ�������±�ʱ�ȴ�
        if (currEnemy.physicscheck.touchWall || !currEnemy.physicscheck.isGround)
        {
            currEnemy.isWait = true;
        }
        else
        {
            currEnemy.isWait = false;
        }

        if (!currEnemy.isWait)
        {
            currEnemy.anim.SetBool("walk", true);
        }
        else
        {
            currEnemy.anim.SetBool("walk", false);
            currEnemy.rb.velocity = new UnityEngine.Vector2(0, 0);
        }

        //�������ʱ�л�chase״̬ 
        if(currEnemy.FoundPlayer())
        {
            currEnemy.switchState(NPCStates.Chase);
        }
    }


    public override void PhysicsUpdata()
    {
        
    }

    public override void OnExit()
    {
        currEnemy.anim.SetBool("walk", false);
    }
}
