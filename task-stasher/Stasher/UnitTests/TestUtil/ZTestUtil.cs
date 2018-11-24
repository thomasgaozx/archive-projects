using System;

namespace TaskStasher.Control.Core.UnitTests
{
    public static class ZTestUtil
    {

        #region Private Fields

        private static Random random = new Random();

        #endregion


        public const string TestTitle = "TestTask";
        public const string TestDescription = "Test Task Description";



        public static string GenerateTestTitle()
        {
            return $"{TestTitle}+{DateTime.Now.Ticks}{random.Next(0,20)}";
        }

        public static string GenerateTestDescription()
        {
            return $"{TestDescription} {DateTime.Now.Ticks} {random.Next(0, 20)}";
        }

    }
}
