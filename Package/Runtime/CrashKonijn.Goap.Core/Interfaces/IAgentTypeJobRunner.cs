using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IAgentTypeJobRunner
    {
        void Run(IReadOnlyList<IMonoGoapActionProvider> queue);
        void Complete();
        void Dispose();
        IGraph GetGraph();
    }
}
