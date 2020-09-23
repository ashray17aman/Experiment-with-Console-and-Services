using System;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ashes
{

    internal class DataProcessor
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        public static bool stopThread = false;
        //private static Timer aTimer;
        private static CancellationTokenSource ts = new CancellationTokenSource();
        private static CancellationToken ct = ts.Token;
        private static Task abc;
        static void ToPass(string[] args)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    Console.Write("exiting from cancel token");
                    Thread.Sleep(2000);
                    break;
                }
                System.Threading.Thread.Sleep(2000);
                Console.Write("wohoo \n");
                throw new CustomException("This exception is expected!");

            }
        }
        internal void Start(string[] arguments)
        {
            try
            {
                abc = Task.Run(() => ToPass(arguments),ct);
                Console.Write("thread started in data processor \n");
            }catch(Exception e)
            {
                Console.Write("got some error \n",e);
            }
        }
        //internal void Execute()
        //{
        //    Console.Write("insite execute \n");
        //    aTimer = new System.Timers.Timer(10000); // 10 Seconds
        //    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        //    aTimer.Enabled = true;
        //}
        //private static void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        ////    DataProcessor dataProcessor = new DataProcessor();
        //    LogMessage();
        //}

        internal void Stop()
        {
            try
            {
                ts.Cancel();
                Console.Write("waiting for abc \n");
                bool result = abc.Wait(5000);
                Console.Write("\n stopping in try " + result + " \n");
                // if result == true, program ended in the specified time, call a new function to clear db connections 
                // if result == false, timeout happened and program did not end, => either it is stuck or needs more time to run
            }
            catch(Exception e)
            {
                Console.Write("currently in exception \n\n");
                // Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
                // clear db connections one by one by checking if its active
            }
            finally
            {
                ts.Dispose();
                Console.Write(" in finally block after disposing task");
            }
        }
        /// <summary>
        /// Writes a simple message to a file in the application directory
        /// </summary>
        //private static void LogMessage()
        //{
        //    string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
        //    string fileName = string.Format(assemblyName + "-{0:yyyy-MM-dd}.log", DateTime.Now);
        //    string filePath = AppDomain.CurrentDomain.BaseDirectory + fileName;

        //    string message = "Execution completed at " + DateTime.Now.ToShortTimeString();

        //    Console.WriteLine(message);
        //    using (StreamWriter sw = new StreamWriter(filePath, true))
        //    {
        //        sw.WriteLine(message);
        //        sw.Close();
        //    }
        //}

    }
}

public class CustomException : Exception
{
    public CustomException(String message) : base(message)
    { }
}