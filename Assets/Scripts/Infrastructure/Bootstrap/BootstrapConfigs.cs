using System;
using Infrastructure.Bootstrap.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Bootstrap
{
    [CreateAssetMenu(
        fileName = "LoadingPipeline",
        menuName = "App/AppLoading/New LoadingPipeline"
    )]
    public class BootstrapConfigs : ScriptableObject
    {
        [FormerlySerializedAs("_tasks")] public Task[] Tasks;
    }
}
