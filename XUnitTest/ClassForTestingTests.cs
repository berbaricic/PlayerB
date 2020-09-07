using SessionControl.Models;
using System;
using Xunit;

namespace XUnitTest
{
    public class ClassForTestingTests
    {
        private readonly ClassForTesting classForTesting;

        public ClassForTestingTests()
        {
            classForTesting = new ClassForTesting();
        }
        [Fact]
       public void MethodTest_ArgumentChecking()
        {
            classForTesting.MethodTest("Zagreb", null);
        }

        [Fact]
        public void MethodTest_LocationChecking()
        {
            classForTesting.MethodTest("Lond4on", "ebfthg3z3535");
        }
    }
}
