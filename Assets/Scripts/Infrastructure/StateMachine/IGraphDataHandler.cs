namespace Infrastructure.StateMachine
{
    internal interface IGraphDataHandler
    {
        public GraphDataStructureInt GrapDataStructureInt { get; set; }
        void ConvertDataToStruct();

        void Execute();
    }
}