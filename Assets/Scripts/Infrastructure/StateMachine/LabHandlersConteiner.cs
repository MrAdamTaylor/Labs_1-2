namespace Infrastructure.StateMachine
{
    public class LabHandlersConteiner : ILabHandlersConteiner
    {
        public ILabExecuter DataCheck<T>(T getData) where T : LBData
        {
            var handler = LBDataDictionaryAdapter<T>.Conteiner.GetHandlerByData(getData);
            return handler;
            //throw new Exception("Класс для подбора реализаций не реализован");
        }
        
    }
}