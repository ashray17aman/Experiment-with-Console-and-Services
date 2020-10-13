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

    public class DataProcessor
    {
        public CancellationTokenSource ts = new CancellationTokenSource();
        private Task abc;
        private Boolean flag = true;

        public virtual void ToPass(string[] args, CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    Console.Write("exiting from cancel token");
                    Thread.Sleep(2000);
                    break;
                }
                InternalExecute(args);
                // Thread.Sleep(2000);
                if (flag)
                {
                    string path = @"D:\fromexecute.txt";
                    string text2write = "Hello World!" + DateTime.Now.ToShortTimeString();
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
                    writer.Write(text2write);
                    writer.Close();
                    flag = false;
                }
            }
        }
        public virtual void InternalExecute(string[] args)
        {
            Console.Write("wohoo inside internal \n");
            // throw new CustomException("This exception is expected!");
            string path = @"D:\fromexecuteinternal.txt";
            string text2write = "Hello World!"+ DateTime.Now.ToShortTimeString();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
            writer.Write(text2write);
            writer.Close();
        }

        public virtual void Start(string[] arguments)
        {
            InvalidOperationException exc = new InvalidOperationException("start called twice");
            //exc.Message = "start called twice";
            //throw new CustomException("start called twice");
            Console.Write("abc is " + abc +"\n");
            if (abc != null)
            {
                Console.Write("Message is " + exc.Message + "\n");
                throw exc;
            }
            CancellationToken ct = ts.Token;
            abc = Task.Run(() => ToPass(arguments, ct), ct);
            Console.Write("started in data processor \n");
            //string path = @"D:\fromstart.txt";
            //string text2write = "Hello World!" + DateTime.Now.ToShortTimeString();
            //System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
            //writer.Write(text2write);
            //writer.Close();
        }

        public virtual void Stop()
        {
            // flag = false;
            try
            {
                ts.Cancel();
                Console.Write("waiting for abc \n");
                Console.Write(abc+" is abc \n");
                bool result = abc.Wait(1000);
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
                Console.Write("currently in exception \n");
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
                // clear db connections one by one by checking if its active
            }
            finally
            {
                ts.Dispose();
                Release();
                Console.Write(" in finally block after disposing task");
            }
        }
        public virtual void Release()
        {
            Console.Write("releasing \n");
        }
    }
    public class CustomException : Exception
    {
        public CustomException(String message) : base(message)
        { }
    }
}

