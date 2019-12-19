using System;

namespace NanoSoft.Test
{
    public class NanoAssert
    {
        public static void Equal<T>(T expected, T actual1, T actual2)
        {
            try
            {
                Xunit.Assert.Equal(expected, actual1);
            }
            catch (Exception e)
            {
                throw new Exception("expected != actual1, " + e.Message);
            }

            try
            {
                Xunit.Assert.Equal(expected, actual2);
            }
            catch (Exception e)
            {
                throw new Exception("expected != actual2, " + e.Message);
            }
        }

        public static void NullOrWhiteSpace(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                throw new Exception($"failed to assert that '{value}' is null or white space.");
        }

        public static void NotNullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"failed to assert that '{value}' is not null or white space.");
        }
    }
}
