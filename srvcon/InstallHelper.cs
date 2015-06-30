// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Security;
using System.ServiceProcess;
using Ninject.Extensions.Logging;

namespace srvcon
{
    internal class InstallHelper
    {
        private readonly ILogger _log;

        public InstallHelper(ILogger log)
        {
            _log = log;
        }

        public static string InstallServiceName = "srvcon";

        public static void Uninstall(UninstallOptions options)
        {
            if (!IsServiceInstalled())
            {
                //log.Error("Service can not be uninstalled. Service is not installed.");
                Environment.Exit(1);
            }

            if (!options.Force)
            {
                Console.WriteLine("Doy you really want to uninstall. Press key (y/n)");
                var key = Console.ReadKey();
                if (key.KeyChar != 'y')
                {
                    //log.Info("Operation canceled by user.");
                    Environment.Exit(1);
                }
            }

            try
            {
                ManagedInstallerClass.InstallHelper(new[] {"/u", Assembly.GetExecutingAssembly().Location});
            }
            catch (InstallException ex)
            {
                if (ex.InnerException is SecurityException)
                {
                    SayNotEnaughRights();
                }
                else
                {
                    throw;
                }
            }
            Environment.Exit(0);
        }

        public static void Install(InstallOptions options)
        {
            if (IsServiceInstalled())
            {
                //log.Warining("Service is already installed");
                Environment.Exit(0);
            }

            try
            {
                ManagedInstallerClass.InstallHelper(new[] {Assembly.GetExecutingAssembly().Location});
            }
            catch (InvalidOperationException ex)
            {
                //TODO
                //Logger.Error(ex.Message);
                //Logger.Debug(ex.ToString());
                if (ex.InnerException is SecurityException)
                {
                    SayNotEnaughRights();
                }
                else
                {
                    throw;
                }
            }
            Environment.Exit(0);
        }

        private static bool IsServiceInstalled()
        {
            return ServiceController
                .GetServices()
                .Any(s => s.ServiceName == InstallServiceName);
        }

        private static void SayNotEnaughRights()
        {
            Console.Error.WriteLine(
                "Not enaugh rights to install/uninstall service. Use Run as to start a program in the context of an administrator account.");
        }
    }
}