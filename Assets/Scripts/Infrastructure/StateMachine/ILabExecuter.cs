using Infrastructure.Services;

namespace Infrastructure.StateMachine
{
    public interface ILabExecuter : IService
    {
        public void Execute();
        void LoadData<T>(T getData) where T : LBData;
    }
}