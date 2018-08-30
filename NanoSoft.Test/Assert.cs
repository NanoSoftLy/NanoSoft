using System;

namespace NanoSoft.Test
{
    public class Assert
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
    }
}
