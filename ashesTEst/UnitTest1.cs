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
        public void TestInternalExecute()
        {
            string[] cars = { "hello", "world" };
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            dp.CallBase = true;
            
            //CancellationTokenSource ts = dp.Object.ts;
            //dp.Setup(mock => mock.Start(cars)).CallBase();
            //dp.Setup(mock => mock.ToPass(cars, ts.Token)).CallBase();
            //dp.Setup(mock => mock.InternalExecute(cars)).CallBase();
            dp.Object.Start(cars);
            dp.Verify(mock => mock.InternalExecute(cars), Times.AtLeastOnce());

        }
        [TestMethod]
        public void TestStart()
        {
            string[] cars = { "hello", "world" };
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            //dp.SetupSequence(f => f.Start(cars))
            //.Returns(3)  // will be returned on 1st invocation
            //.Returns(2)  // will be returned on 2nd invocation
            //.Returns(1)  // will be returned on 3rd invocation
            //.Returns(0)  // will be returned on 4th invocation
            //.Throws(new InvalidOperationException());  // will be thrown on 5th invocation
            dp.Setup(foo => foo.Start(cars)).Throws<System.Exception>();
            dp.Object.Start(cars);
            dp.Object.Start(cars);
            dp.Verify(mock => mock.Start(cars), Times.AtLeastOnce());

        }
    }
}
