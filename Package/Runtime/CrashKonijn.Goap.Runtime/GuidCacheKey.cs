using System;
using System.Collections.Generic;
using System.Text;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public static class GuidCacheKey
    {
        private const int MaskBits = 64;

        private static readonly StringBuilder StringBuilder = new();

        // Goal indexes are 0..N-1 per agent type, so a goal set fits in a bit mask: each set builds its key string once.
        private static readonly Dictionary<ulong, string> KeysByMask = new();

        public static string GenerateKey(List<IGoal> guids)
        {
            if (guids == null || guids.Count == 0)
                throw new ArgumentException("Guid list must not be null or empty.");

            SortByIndex(guids);

            if (!TryGetMask(guids, out var mask))
                return BuildKey(guids);

            if (!KeysByMask.TryGetValue(mask, out var key))
            {
                key = BuildKey(guids);
                KeysByMask[mask] = key;
            }

            return key;
        }

        private static bool TryGetMask(List<IGoal> guids, out ulong mask)
        {
            mask = 0;

            foreach (var guid in guids)
            {
                if (guid.Index < 0 || guid.Index >= MaskBits)
                    return false;

                mask |= 1UL << guid.Index;
            }

            return true;
        }

        private static string BuildKey(List<IGoal> guids)
        {
            StringBuilder.Clear();

            foreach (var guid in guids)
            {
                StringBuilder.Append(guid.Index);
                StringBuilder.Append("-");
            }

            return StringBuilder.ToString();
        }

        // Insertion sort: goal lists are tiny, and Mono's List.Sort allocates on every call.
        private static void SortByIndex(List<IGoal> guids)
        {
            for (var i = 1; i < guids.Count; i++)
            {
                var goal = guids[i];
                var j = i - 1;

                while (j >= 0 && guids[j].Index > goal.Index)
                {
                    guids[j + 1] = guids[j];
                    j--;
                }

                guids[j + 1] = goal;
            }
        }
    }
}
