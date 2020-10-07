using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ashes;
using System.Threading;

namespace AshesTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] cars = { "hello", "world" };
            var mockPrg = new Mock<Program>();
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            mockPrg.CallBase = true;
            dp.CallBase = true;
            // dp.Setup(mock => mock.Start(cars));
            // mockPrg.Setup(mock => mock.RunAsAConsole(cars, dp.Object));
            mockPrg.Object.RunAsAConsole(cars, dp.Object);
            dp.Object.Stop();
            mockPrg.Verify(mock => mock.RunAsAConsole(cars, dp.Object), Times.Once());
            dp.Verify(mock => mock.Start(cars), Times.Once());
            dp.Verify(mock => mock.ToPass(cars), Times.Once());
        }
    }
}
