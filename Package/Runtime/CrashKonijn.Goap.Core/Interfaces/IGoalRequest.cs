using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IGoalRequest
    {
        List<IGoal> Goals { get; }
        public string Key { get; set; }
    }

    public interface IGoalResult
    {
        IGoal Goal { get; }
        IReadOnlyList<IConnectable> Plan { get; }
        IGoapAction Action { get; }
    }
}