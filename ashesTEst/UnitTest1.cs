using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ashes;

namespace AshesTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] cars = { "hello", "world" };
            Mock<Program> mockPrg = new Mock<Program>();
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            mockPrg.Setup(mock => mock.RunAsAConsole(cars, dp.Object)).CallBase();
            dp.Setup(mock => mock.Start(cars));
            mockPrg.Object.RunAsAConsole(cars, dp.Object);
            mockPrg.Verify(mock => mock.RunAsAConsole(cars, dp.Object), Times.Once());
            dp.Verify(mock => mock.Start(cars), Times.Once());
        }
    }
}
