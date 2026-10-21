using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class AgentCollection : IAgentCollection
    {
        private readonly IAgentType agentType;
        private HashSet<IMonoGoapActionProvider> agents = new();
        private HashSet<IMonoGoapActionProvider> queue = new();
        private readonly List<IMonoGoapActionProvider> queueBuffer = new();

        public AgentCollection(IAgentType agentType)
        {
            this.agentType = agentType;
        }

        public HashSet<IMonoGoapActionProvider> All() => this.agents;

        public void Add(IMonoGoapActionProvider actionProvider)
        {
            if (!actionProvider.isActiveAndEnabled)
                return;

            if (this.agents.Contains(actionProvider))
                return;

            this.agents.Add(actionProvider);
            this.agentType.Events.AgentRegistered(actionProvider);
        }

        public void Remove(IMonoGoapActionProvider actionProvider)
        {
            if (!this.agents.Contains(actionProvider))
                return;

            this.agents.Remove(actionProvider);
            this.agentType.Events.AgentUnregistered(actionProvider);
        }

        public void Enqueue(IMonoGoapActionProvider actionProvider)
        {
            if (!this.agents.Contains(actionProvider))
                return;

            this.queue.Add(actionProvider);
        }

        public int GetQueueCount() => this.queue.Count;

        // Reused buffer: valid until the next GetQueue call. Agents enqueued while it is read go to the next frame.
        public IReadOnlyList<IMonoGoapActionProvider> GetQueue()
        {
            this.queueBuffer.Clear();
            this.queueBuffer.AddRange(this.queue);

            this.queue.Clear();

            return this.queueBuffer;
        }
    }
}
