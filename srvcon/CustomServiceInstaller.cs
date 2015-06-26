using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace srvcon
{
    [RunInstaller(true)]
    public class CustomServiceInstaller : Installer
    {
        public CustomServiceInstaller()
        {
            var process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };


            var service = new ServiceInstaller
            {
                ServiceName = Program.InstallServiceName
            };

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}