using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    class BfsShortestPath : IBfs
    {
        private IStructGraphDataIntReader _graphStructReader;
        private Queue<int> _queue;
        private int Size;
        private bool[] visited;
        private int[] dist;
        private int[] parent;

        public BfsShortestPath(GraphDataStructureInt structData, GraphData data)
        {
            Size = data.Size;
            _graphStructReader = new StructGraphDataIntReader(structData);
            _queue = new Queue<int>();
        }

        public void Run(int source)
        {
            visited = new bool[Size];
            dist = new int[Size];
            parent = new int[Size];

            InitParent();
        
            _queue.Enqueue(source);
            bool temp = true;
            int sourcesIndex = source - _graphStructReader.GetShift();
            visited[sourcesIndex] = temp;
            parent[sourcesIndex] = source;
            dist[sourcesIndex] = 0;

            while (_queue.Count != 0)
            {
                int first = _queue.Dequeue();
                //Debug.Log("(A) BFS: visited: "+first);

                foreach (int neighbor in _graphStructReader.GetNeighboursByIndex(first))
                {
                    int shiftedIndex = first - _graphStructReader.GetShift();
                    int shiftedNeighbourIndex = neighbor - _graphStructReader.GetShift();
                    if (!visited[shiftedNeighbourIndex])
                    {
                        _queue.Enqueue(neighbor);
                        parent[shiftedNeighbourIndex] = first;
                        dist[shiftedNeighbourIndex] = dist[shiftedIndex] + 1;
                        visited[shiftedNeighbourIndex] = true;
                    }
                } 
            }
        }

        private void InitParent()
        {
            for (int i = 0; i < Size; i++)
            {
                parent[i] = -1;
            }
        }


        public int ReturnShortestPath(int source, int destinationPoint = -1)
        {
            int distance = 0;

            if (destinationPoint != -1)
            {
                if (source == destinationPoint)
                {
                    return distance;
                }
                else
                {
                    Run(source);
                    int index = destinationPoint - _graphStructReader.GetShift();
                    distance = dist[index];
                }
                
            }
            else
            {
                Debug.Log("Нет точки назначения для BFS");
            }
            return distance;
        }
    }
}