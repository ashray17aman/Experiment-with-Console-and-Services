using System;
using System.ServiceProcess;
using System.Runtime.InteropServices;
namespace Ashes
{
    public class Program
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        //[DllImport("kernel32.dll")]
        //public static extern Boolean AttachConsole(int pid);
        static void Main(string[] args)
        {
            if ((!Environment.UserInteractive))
            {
                Program.RunAsAService();
            }
            else
            {
                if (args != null && args.Length > 0)
                {
                    if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                    {
                        SelfInstaller.InstallMe();
                    }
                    else
                    {
                        if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                        {
                            SelfInstaller.UninstallMe();
                        }
                        else
                        {
                            Console.WriteLine("Invalid argument!");
                        }
                    }
                }
                else
                {
                    Program pp = new Program();
                    pp.RunAsAConsole(args);

                }
            }
        }

        public virtual void RunAsAConsole(string[] args,DataProcessor dp=null)
        {
            AllocConsole();
            //Console.Write(AllocConsole());
            //Console.Write(AttachConsole(-1));

            DataProcessor dataProcessor = new DataProcessor();
            dataProcessor.Start(args);
            Console.Write("Started in console, write exit to close \n");
            String input = String.Empty;
            // while (input.ToLower() != "exit") input = Console.ReadLine();
            dataProcessor.Stop();
        }
        static void RunAsAService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
           {
                new HybridSvxService()
           };
            ServiceBase.Run(servicesToRun);
        }


    }
}