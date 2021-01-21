using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod, TestCategory("Smoke")]
        public void TestMethod1()
        {
            Console.WriteLine("Test method 1");
        }

        [TestMethod]
        [Ignore]
        public void TestMethod2()
        {
            Console.WriteLine("Test method 2");
        }

        [TestInitialize]
        public void SetUp()
        {
            Console.WriteLine("This is set up");
        }

        [TestCleanup]
        public void TearDown()
        {
            Console.WriteLine("This is clean up");
        }

        [ClassInitialize]
        public static void ClassSetup(TestContext testContext)
        {
            Console.WriteLine("Class setup");
        }

        [ClassCleanup]
        public static void ClassTeardown()
        {
            Console.WriteLine("Class cleanup");
        }

        [AssemblyInitialize]
        public static void AssemblySetup(TestContext testContext)
        {
            Console.WriteLine("Assembly setup");
        }

        [AssemblyCleanup]
        public static void AssemblyTeardown()
        {
            Console.WriteLine("Assembly cleanup");
        }
    }
}
