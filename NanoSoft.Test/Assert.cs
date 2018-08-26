
using System;

namespace NanoSoft.Test
{
    public class Assert
    {
        public static void Equal<T>(T expected, T actual1, T actual2)
        {
            if (!actual1.Equals(expected) || !actual2.Equals(expected))
                throw new Exception("failed to assert that both actual values equals expected");
        }
    }
}
