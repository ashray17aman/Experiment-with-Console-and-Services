using System;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Runtime.InteropServices;

namespace Ashes
{

    internal class DataProcessor
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        private static Timer aTimer;
        internal void toPass()
        {
            AllocConsole();
            while (true)
            {
                System.Threading.Thread.Sleep(10000);
                LogMessage();
                Console.Write("wohoo");
            }
            //FreeConsole();
        }
        internal void Start(string[] arguments)
        {
            try
            {
                Thread abc = new Thread(new ThreadStart(this.toPass));
                abc.Start();
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
            aTimer.Stop();
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