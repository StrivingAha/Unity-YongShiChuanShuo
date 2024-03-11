
public abstract class BaseState
{
    protected Enemy currEnemy;
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdata();
    public abstract void PhysicsUpdata();
    public abstract void OnExit();
}
