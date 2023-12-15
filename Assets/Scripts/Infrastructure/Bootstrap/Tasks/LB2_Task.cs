using DefaultNamespace;
using Infrastructure;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(
    fileName = "LB2",
    menuName = "App/Tasks/LB2"
)]
class LB2_Task : Task
{
    public bool undirectGraph = true;
    public AdjacenyListStruct[] GraphData;
    
    public override void Run()
    {
        Debug.Log("Вторая лабораторная загружается!");
        //Debug.Log(this.GetType());
    }
    
    public override LBData GetData()
    {
        GraphData graphData = new GraphData(this.GraphData.Length);
        int[] dataArray = this.GraphData.ConvertToArray(this.GraphData);
        graphData.Size = graphData.Size.ReturnSizeUniqueNumbers(dataArray);
        graphData.GraphStructure = graphData.GraphStructure.ReturnUniqueArrayNumbers(dataArray);
        graphData.Undir = this.undirectGraph;
        Debug.Log("Проверка расширения!");
        Debug.Log("Размер графа: " + graphData.Size);
        for (int i = 0; i < GraphData.Length; i++)
        {
            graphData.Pairs[i] = CreatePairData();
            graphData.Pairs[i].GraphPoint = this.GraphData[i].graph;
            graphData.Pairs[i].NeighborPoints = this.GraphData[i].neighbours;
        }
        return graphData;
    }

    private GraphData.GraphIntPairs CreatePairData()
    {
        Infrastructure.GraphData.GraphIntPairs pair = new GraphData.GraphIntPairs();
        return pair;
    }
}
[System.Serializable]
public struct AdjacenyListStruct
{
    public int graph;
    public int[] neighbours;
}
