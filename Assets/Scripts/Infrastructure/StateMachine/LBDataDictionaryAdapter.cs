using System;
using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    public class LBDataDictionaryAdapter<T> where T : LBData
    {
        private static LBDataDictionaryAdapter<T> _conteiner;
        public static LBDataDictionaryAdapter<T> Conteiner
        {
            get
            {
                if (_conteiner == null)
                {
                    _conteiner = new LBDataDictionaryAdapter<T>();
                }
                return _conteiner;
            }
        }

        private List<ILabExecuter> _labsInterfaces = new List<ILabExecuter>();

        public ILabExecuter GetHandlerByData(T data) 
        {
            var index = OrderType(data);
            
            switch (index)
            {
                case 0:  
                    return LBDataDictionary.Conteiner.GetHandlerByData(data);
                case 1: 
                    return LBDataDictionary.Conteiner.GetHandlerByData(data);;
                default:
                    throw new Exception("Добавть новый индекс для данных, " +
                                        "так как интеграция словаря с OCP принципом реализована не достаточно хорошо!");
            }
        }

        private int OrderType(T data)
        {
            if (data.GetType() == typeof(MgyaData))
            {
                return 0;
            }
            else if(data.GetType() == typeof(GraphData))
            {
                return 1;
            }
            else
            {
                throw new Exception("Добавьте ещё один индекс");
            }
        }
    }
}