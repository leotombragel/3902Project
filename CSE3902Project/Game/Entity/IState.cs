namespace CSE3902Project.Game.Entity;

public interface IState
{
    string AnimationName { get; }
    void Enter();
    void Update(float deltaTime);
    void Exit();
}