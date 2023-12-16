using Infrastructure.Services;

namespace Infrastructure.StateMachine
{
    interface ILabHandlersConteiner : IService
    {
        public ILabExecuter DataCheck<T>(T getData) where T : LBData;
    }
}