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
        private CancellationTokenSource ts = new CancellationTokenSource();
        private Task abc;

       

        static void ToPass(string[] args,CancellationToken ct)
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
                // throw new CustomException("This exception is expected!");

            }
        }
        internal void Start(string[] arguments)
        {        
            CancellationToken ct = ts.Token;
            abc = Task.Run(() => ToPass(arguments,ct),ct);
            Console.Write("thread started in data processor \n");
        }

        internal void Stop()
        {
            try
            {
                ts.Cancel();
                Console.Write("waiting for abc \n");
                bool result = abc.Wait(5000);
                Console.Write("\n stopping in try " + result + " \n");
                if (result == false)
                {
                    Console.Write("timeout failed, need to force cancel");
                }
                // if result == true, program ended in the specified time, call a new function to clear db connections 
                // if result == false, timeout happened and program did not end, => either it is stuck or needs more time to run
            }
            catch(Exception e)
            {
                Console.Write("currently in exception");
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
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