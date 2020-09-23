using System.ServiceProcess;

namespace Ashes
{
    partial class HybridSvxService : ServiceBase
    {
        DataProcessor dataProcessor;

        public HybridSvxService()
        {
            InitializeComponent();
            dataProcessor = new DataProcessor();
        }

        protected override void OnStart(string[] args)
        {
            //dataProcessor.Execute();
        }


        protected override void OnStop()
        {
            dataProcessor.Stop();
        }

    }
}