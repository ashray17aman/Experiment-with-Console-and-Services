using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Ashes
{
    [RunInstaller(true)]
    public class HybridSvxServiceInstaller : Installer
    {
        public HybridSvxServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            // Setup the Service Account type per your requirement
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            serviceInstaller.ServiceName = "Ashes";
            serviceInstaller.DisplayName = "Ashes Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.Description = "My custom hybrid service";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }

    }
}