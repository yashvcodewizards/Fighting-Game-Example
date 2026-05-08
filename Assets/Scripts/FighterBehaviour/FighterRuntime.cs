namespace FighterBehaviour
{
    /// <summary>
    /// Runtime container passed through the fighter state machine.
    /// Groups together the fighter's context, services, and queries so states
    /// can operate without depending directly on the controller.
    /// </summary>
    public sealed class FighterRuntime
    {
        public FighterServices Services { get; }
        public FighterContext Context { get; }
        public FighterQueries Queries { get; }

        public FighterRuntime(FighterServices services, FighterContext context)
        {
            Services = services;
            Context = context;
            Queries = new FighterQueries(services, context);
        }
    }
}