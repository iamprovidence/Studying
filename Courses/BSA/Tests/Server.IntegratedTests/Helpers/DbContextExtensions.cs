using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using System.Linq;

namespace Server.IntegratedTests.Helpers
{

    // this class is required because InMemoryDb 
    // does not know how to increase ID
    //
    // To use, call context.ResetValueGenerators(); 
    // before the context is used for the first time and any time that EnsureDeleted is called.
    public static class DbContextExtensions
    {
        public static void ResetValueGenerators(this DbContext context)
        {
            IValueGeneratorCache cache = context.GetService<IValueGeneratorCache>();

            foreach (IProperty keyProperty in context.Model.GetEntityTypes()
                .Select(e => e.FindPrimaryKey().Properties[0])
                .Where(p => p.ClrType == typeof(int) && p.ValueGenerated == ValueGenerated.OnAdd))
            {
                ResettableValueGenerator generator = (ResettableValueGenerator)cache.GetOrAdd(
                                                        keyProperty,
                                                        keyProperty.DeclaringEntityType,
                                                        (p, e) => new ResettableValueGenerator());

                generator.Reset();
            }
        }
    }
}
