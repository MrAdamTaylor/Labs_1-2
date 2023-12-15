namespace Infrastructure.StateMachine
{
    internal interface IState
    {
        void Enter();
        
        void Exit();
    }
}