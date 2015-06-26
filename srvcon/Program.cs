// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using CommandLine;
using System;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace srvcon
{

    internal static class CommandNames
    {
        public const string Install = "install";
        public const string Uninstall = "uninstall";
        public const string Console = "console";
    }


    internal class Options
    {
        public Options()
        {

        }

        [VerbOption("install", HelpText = "Record changes to the repository.")]
        public InstallOptions Install { get; set; }

        [VerbOption("uninstall", HelpText = "Update remote refs along with associated objects.")]
        public UninstallOption Uninstall { get; set; }

        [VerbOption("console", HelpText = "Update remote refs along with associated objects.")]
        public ConsoleOptions Console { get; set; }
    }

    internal class ConsoleOptions
    {
    }

    internal class UninstallOption
    {
    }

    internal class InstallOptions
    {
    }


    internal class Program : ServiceBase
    {
        public static string InstallServiceName = "srvcon";




        private static void Main(string[] args)
        {


            var options = new Options();
            bool isOk = Parser.Default.ParseArguments(args, options, Execute);
            if (!isOk)
            {
                Environment.Exit(Parser.DefaultExitCodeFail);
            }
            else
            {
            }

            bool debugMode = false;
            if (args.Length > 0)
            {
                for (int ii = 0; ii < args.Length; ii++)
                {
                    switch (args[ii].ToUpper())
                    {
                        case "/NAME":
                            if (args.Length > ii + 1)
                            {
                                InstallServiceName = args[++ii];
                            }
                            break;
                        case "/I":
                            InstallService();
                            return;
                        case "/U":
                            UninstallService();
                            return;
                        case "/D":
                            debugMode = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (debugMode)
            {
                Program service = new Program();
                service.OnStart(null);
                Console.WriteLine("Service Started...");
                Console.WriteLine("<press any key to exit...>");
                Console.Read();
            }
            else
            {
                Run(new Program());
            }
        }

        private static void Execute(string command, object options)
        {
            switch (command)
            {
                
            }

        }

        protected override void OnStart(string[] args)
        {
            //start any threads or http listeners etc
        }

        protected override void OnStop()
        {
            //stop any threads here and wait for them to be stopped.
        }

        private static bool IsServiceInstalled()
        {
            return ServiceController
                .GetServices()
                .Any(s => s.ServiceName == InstallServiceName);
        }

        private static void InstallService()
        {
            if (IsServiceInstalled())
            {
                UninstallService();
            }

            ManagedInstallerClass.InstallHelper(new[] {Assembly.GetExecutingAssembly().Location});
        }

        private static void UninstallService()
        {
            ManagedInstallerClass.InstallHelper(new[] {"/u", Assembly.GetExecutingAssembly().Location});
        }
    }
}