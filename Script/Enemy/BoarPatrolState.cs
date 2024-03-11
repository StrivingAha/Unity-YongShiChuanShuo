

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
        //敌人碰到墙或在悬崖边时等待
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

        //发现玩家时切换chase状态 
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
