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
        private static Timer aTimer;
        private static CancellationTokenSource ts = new CancellationTokenSource();
        private static CancellationToken ct = ts.Token;
        private static Task abc;
        static void ToPass(string[] args)
        {
            //AllocConsole();
            while (true)
            {
                //ct.ThrowIfCancellationRequested();
                if (ct.IsCancellationRequested)
                {
                    Console.Write("exiting from cancel token");
                    Thread.Sleep(2000);
                    break;
                }
                //if (stopThread)
                //  break;
                System.Threading.Thread.Sleep(2000);
                // LogMessage();
                Console.Write("wohoo");
            }
            Console.Write("outside while true loop");
            //FreeConsole();
        }
        internal void Start(string[] arguments)
        {
            try
            {
                //CancellationToken ct = ts.Token;
                abc = Task.Run(() => ToPass(arguments),ct);
                // abc.Start(arguments);
                Console.Write("thread started in data processor");
            }catch(Exception e)
            {
                Console.Write("got some error");
            }
        }
        internal void Execute()
        {
            Console.Write("insite execute \n");
            aTimer = new System.Timers.Timer(10000); // 10 Seconds
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
        //    DataProcessor dataProcessor = new DataProcessor();
            LogMessage();
        }

        internal void Stop()
        {
            if(aTimer!=null)
                aTimer.Stop();
            //stopThread = true;
            ts.Cancel();
            //Thread.Sleep(5000);
            Console.Write("waiting for abc");
            abc.Wait();
            Console.Write("stopping \n");
            try
            {
                // abc;
                Console.Write(" in try");
            }
            catch(OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");

            }
            finally
            {
                ts.Dispose();
                Console.Write(" in finally block");
            }
        }
        /// <summary>
        /// Writes a simple message to a file in the application directory
        /// </summary>
        private static void LogMessage()
        {
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            string fileName = string.Format(assemblyName + "-{0:yyyy-MM-dd}.log", DateTime.Now);
            string filePath = AppDomain.CurrentDomain.BaseDirectory + fileName;

            string message = "Execution completed at " + DateTime.Now.ToShortTimeString();

            Console.WriteLine(message);
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(message);
                sw.Close();
            }
        }

    }
}