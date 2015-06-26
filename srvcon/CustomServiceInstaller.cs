// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

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
                ServiceName = InstallHelper.InstallServiceName
            };

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}