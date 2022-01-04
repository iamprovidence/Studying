using System;
using System.Collections.Generic;

using static BusinessLayer.Test.Helpers.DefaultFakeDataInitializer;

using Date = System.DateTime;

namespace BusinessLayer.Test.Helpers
{
    public static class RandomDataDrivenProvider
    {
        static Random Random { get; } = new Random();

        // GENERATORS
        // user id
        public static IEnumerable<object[]> RandomUserId
        {
            get
            {
                for (int i = 0; i < USERS_AMOUNT; ++i)
                {
                    yield return new object[] { Random.Next(USERS_AMOUNT) + 1 };
                }
            }
        }
        // user id - max task length
        public static IEnumerable<object[]> RandomUserIdAndTaskLength
        {
            get
            {
                for (int i = 0; i < USERS_AMOUNT; ++i)
                {
                    yield return new object[] { Random.Next(USERS_AMOUNT) + 1, Random.Next(50) };
                }
            }
        }
        // user id - year
        public static IEnumerable<object[]> RandomUserIdAndYear
        {
            get
            {
                for (int i = 0; i < USERS_AMOUNT; ++i)
                {
                    yield return new object[] { Random.Next(USERS_AMOUNT) + 1, Date.Now.AddYears(Random.Next(-5, 5)).Year };
                }
            }
        }
    }
}
