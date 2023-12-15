using System;
using System.Collections.Generic;
using Infrastructure.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure
{
    public class DataConteiner
    {
        private static DataConteiner _conteiner;
        public static DataConteiner Conteiner
        {
            get
            {
                if (_conteiner == null)
                {
                    _conteiner = new DataConteiner();
                }
                return _conteiner;
            }
        }

        public List<LBData> Data = new List<LBData>();

        public int LBCount;

        public void AddData(LBData data)
        {
            Data.Add(data);
        }

        public LBData GetDataByIndex(int index)
        {
            return Data[index];
        }
    }

    public class LBDataDictionary
    {
        private static LBDataDictionary _conteiner;
        public static LBDataDictionary Conteiner
        {
            get
            {
                if ( _conteiner == null )
                {
                    _conteiner = new LBDataDictionary();
                }

                return _conteiner;
            }
        }

        private readonly Dictionary<Type, ILabExecuter> _dictionary = new();

        public void AddDataType(LBData data, ILabExecuter executer)
        {
            if (_dictionary.ContainsKey(data.GetType()) || _dictionary.ContainsValue(executer))
            {
                Debug.Log("Такой тип уже существует");
            }
            else
            {
                _dictionary.Add(data.GetType(),executer);
            }
        }

        public ILabExecuter GetHandlerByData(LBData data)
        {
            ILabExecuter executer;
            if (_dictionary.TryGetValue(data.GetType(), out executer))
            { 
                return executer;
            }

            return default;
        }
    }
}