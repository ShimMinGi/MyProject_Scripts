public interface IState
{
    void Enter();
    void Exit();
    void Update();
    void HandleInput();
    void PhysicsUpdate();
}