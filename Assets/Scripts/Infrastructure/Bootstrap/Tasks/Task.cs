using System;
using Infrastructure;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Task : ScriptableObject
{
    public virtual void Run()
    {
        Debug.Log("Я родился!");
    }

    public virtual Task ReturnSelfType()
    {
        return this;
    }

    public virtual LBData GetData()
    {
        var myType = new LBData();
        return myType;
    }
}