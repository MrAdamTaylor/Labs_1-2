namespace Infrastructure.StateMachine
{
    internal interface IBfs
    {
        public void Run(int source);

        public int ReturnShortestPath(int source, int destinationPoint = -1);
    }
}