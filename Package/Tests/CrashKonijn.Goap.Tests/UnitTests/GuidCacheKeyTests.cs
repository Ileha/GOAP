using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    public class GuidCacheKeyTests
    {
        [Test]
        public void GenerateKey_SameGoalSetInAnyOrder_ReturnsSameCachedString()
        {
            // Arrange
            var goal1 = CreateGoal(1);
            var goal3 = CreateGoal(3);

            // Act
            var first = GuidCacheKey.GenerateKey(new List<IGoal> { goal3, goal1 });
            var second = GuidCacheKey.GenerateKey(new List<IGoal> { goal1, goal3 });

            // Assert
            Assert.AreEqual("1-3-", first);
            Assert.AreSame(first, second);
        }

        [Test]
        public void GenerateKey_IndexAboveMask_BuildsKey()
        {
            // Arrange
            var goals = new List<IGoal> { CreateGoal(70), CreateGoal(2) };

            // Act
            var key = GuidCacheKey.GenerateKey(goals);

            // Assert
            Assert.AreEqual("2-70-", key);
        }

        private static IGoal CreateGoal(int index)
        {
            var goal = Substitute.For<IGoal>();
            goal.Index.Returns(index);
            return goal;
        }
    }
}
