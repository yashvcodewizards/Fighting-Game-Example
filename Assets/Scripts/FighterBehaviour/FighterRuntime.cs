namespace FighterBehaviour
{
    public sealed class FighterRuntime
    {
        public FighterServices Services { get; }
        public FighterBehaviourContext Context { get; }
        public FighterQueries Queries { get; }

        public FighterRuntime(FighterServices services, FighterBehaviourContext context)
        {
            Services = services;
            Context = context;
            Queries = new FighterQueries(services, context);
        }
    }
}