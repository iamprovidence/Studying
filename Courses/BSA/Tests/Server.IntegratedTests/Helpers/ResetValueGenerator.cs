using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using System.Threading;

namespace Server.IntegratedTests.Helpers
{
    // this class is required because InMemoryDb 
    // does not know how to increase ID
    public class ResettableValueGenerator : ValueGenerator<int>
    {
        private int current;

        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry) => Interlocked.Increment(ref current);

        public void Reset() => current = 0;
    }
}
