using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LB1",
    menuName = "App/Tasks/LB1"
)]
public class LB1_Task : Task
{
    public EstimateRow[] data;
    
    public override void Run()
    {
        Debug.Log("Первая лабораторная загружается!");
        //Debug.Log(this.GetType());
    }

    public override LBData GetData()
    {
        var myData = new MgyaData();
        myData.Value = new int[data.Length, data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i]._values.Length; j++)
            {
                myData.Value[i, j] = data[i]._values[j];
            }
        }
        return myData;
    }
}

[System.Serializable]
public struct EstimateRow
{
    public int[] _values;
}
