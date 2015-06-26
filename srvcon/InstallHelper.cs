// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace srvcon
{
    internal class InstallHelper
    {
        public static string InstallServiceName = "srvcon";

        public static void Uninstall(UninstallOptions options)
        {
            ManagedInstallerClass.InstallHelper(new[] {"/u", Assembly.GetExecutingAssembly().Location});
        }

        public static void Install(InstallOptions options)
        {
            if (IsServiceInstalled())
            {
                Uninstall(new UninstallOptions {Force = true});
            }

            ManagedInstallerClass.InstallHelper(new[] {Assembly.GetExecutingAssembly().Location});
        }

        private static bool IsServiceInstalled()
        {
            return ServiceController
                .GetServices()
                .Any(s => s.ServiceName == InstallServiceName);
        }
    }
}