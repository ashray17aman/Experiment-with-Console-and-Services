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
            // dp.Object.Stop();
            mockPrg.Verify(mock => mock.RunAsAConsole(cars, dp.Object), Times.Once());
            dp.Verify(mock => mock.Start(cars), Times.Once());
            dp.Verify(mock => mock.ToPass(cars,dp.Object.ts.Token), Times.Once());
        }

        [TestMethod]
        public void TestStop()
        {
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            dp.Setup(mock => mock.Stop()).CallBase();
            dp.Setup(mock => mock.Release());
            dp.Object.Stop();
            dp.Verify(mock => mock.Release(), Times.Once());
        }

        [TestMethod]
        public void TestStart()
        {
            string[] cars = { "hello", "world" };
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            // dp.CallBase = true;
            CancellationTokenSource ts = dp.Object.ts;

            dp.Setup(mock => mock.Start(cars)).CallBase();
            dp.Setup(mock => mock.ToPass(cars, ts.Token)).CallBase();
            dp.Setup(mock => mock.InternalExecute(cars)).CallBase();
            dp.Object.Start(cars);
            dp.Verify(mock => mock.InternalExecute(cars), Times.Once());

        }
    }
}
