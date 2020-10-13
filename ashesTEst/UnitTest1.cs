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
            mockPrg.Object.RunAsAConsole(cars, dp.Object);
            mockPrg.Verify(mock => mock.RunAsAConsole(cars, dp.Object), Times.Once());
            dp.Verify(mock => mock.Start(cars), Times.Once());
            dp.Verify(mock => mock.ToPass(cars,dp.Object.ts.Token), Times.Once());
        }

        [TestMethod]
        public void TestStop()
        {
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            string[] cars = { "hello", "world" };
            dp.Setup(mock => mock.Stop()).CallBase();
            dp.Setup(mock => mock.Start(cars)).CallBase();
            dp.Setup(mock => mock.Release()).CallBase();
            dp.Object.Start(cars);
            dp.Object.Stop();
            dp.Verify(mock => mock.Release(), Times.Once());
        }

        [TestMethod]
        public void TestInternalExecute()
        {
            string[] cars = { "hello", "world" };
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            //dp.CallBase = true;

            CancellationTokenSource ts = dp.Object.ts;
            dp.Setup(mock => mock.Start(cars)).CallBase();
            dp.Setup(mock => mock.ToPass(cars, ts.Token)).CallBase();
            dp.Setup(mock => mock.InternalExecute(cars));
            dp.Object.Start(cars);
            dp.Verify(mock => mock.InternalExecute(cars), Times.AtLeastOnce());

        }
        [TestMethod]
        public void TestStart()
        {
            string[] cars = { "hello", "world" };
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
            ////dp.SetupSequence(f => f.Start(cars))
            ////.Returns(3)  // will be returned on 1st invocation
            ////.Returns(2)  // will be returned on 2nd invocation
            ////.Returns(1)  // will be returned on 3rd invocation
            ////.Returns(0)  // will be returned on 4th invocation
            ////.Throws(new InvalidOperationException());  // will be thrown on 5th invocation
            //dp.Setup(mock => mock.Start(cars)).CallBase();
            //dp.Setup(mock => mock.Start(cars)).Throws<System.Exception>();
            //dp.Setup(mock => mock.Stop()).CallBase();            
            ////dp.Object.Start(cars);
            //dp.Object.Stop();
            ////dp.Verify(mock => mock.Start(cars), Times.AtLeastOnce());
            //dp.Verify(mock => mock.Release(),Times.Once());

            //dp.Setup(mock => mock.Start(cars)).Throws<InvalidOperationException> ();
            dp.Setup(mock => mock.Start(cars)).CallBase();
            dp.Object.Start(cars);
            dp.Verify(mock => mock.Start(cars), Times.Once());
            var exc = Assert.ThrowsException<InvalidOperationException>(() => dp.Object.Start(cars));
            Assert.AreEqual("start called twice", exc.Message);
        }

        public void TestException()
        {
            string[] cars = { "hello", "world" };
            Mock<DataProcessor> dp = new Mock<DataProcessor>();
        }
    }
}
