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
        private CancellationTokenSource ts;
        public CancellationTokenSource tokenSource
        {
            get
            {
                return ts;
            }
            set
            {
                ;
            }
        }
        private Task abc;
        private Boolean flag;

        public DataProcessor()
        {
            ts = new CancellationTokenSource();
            flag = true;
        }

        public virtual void ToPass(string[] args, CancellationToken ct)
        {
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                InternalExecute(args);
                Console.Write("Currently in topass \n");
                Thread.Sleep(2000);
            }
        }
        public virtual void InternalExecute(string[] args)
        {
            Console.Write("wohoo inside internal \n");
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
        }

        public virtual void Stop()
        {
            // flag = false;
            try
            {
                ts.Cancel();
                Console.Write("waiting for abc \n");
                bool result = abc.Wait(-1);
                Console.Write(abc + " is abc \n");
                // Console.Write(abc.Exception.Message + " is exception\n");
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
                if(e is AggregateException)
                {
                    AggregateException ex = (AggregateException)e;
                    foreach (Exception exe in ex.InnerExceptions)
                    {
                        if(exe is OperationCanceledException)
                            Console.WriteLine(" operation cancelled exception ",exe.Message);
                        else
                        {
                            Console.Write("currently in exception \n");
                            Console.WriteLine(" thrown with message: ", e.Message);
                            // clear db connections one by one by checking if its active
                        }
                    }
                }
                else
                {
                    Console.Write("currently in exception \n");
                    Console.WriteLine(" thrown with message: ", e.Message);
                    // clear db connections one by one by checking if its active
                }
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

