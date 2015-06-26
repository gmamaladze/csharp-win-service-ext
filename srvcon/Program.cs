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

        [VerbOption(CommandNames.Install, HelpText = "Install the service.")]
        public InstallOptions Install { get; set; }

        [VerbOption(CommandNames.Uninstall, HelpText = "Uninstall the service.")]
        public UninstallOptions Uninstall { get; set; }

        [VerbOption(CommandNames.Console, HelpText = "Start service as a console application.")]
        public ConsoleOptions Console { get; set; }
    }

    internal class ConsoleOptions
    {
    }

    internal class UninstallOptions
    {
        [Option('f', "force")]
        public bool Force { get; set; }
    }

    internal class InstallOptions
    {
        [Option('u', "user")]
        public string User { get; set; }

        [Option('p', "password")]
        public string Password { get; set; }

    }

    internal class Settings
    {
        private static Settings _instance;

        public static Settings Instance()
        {
            return _instance ?? (_instance = new Settings());
        }

        public int ? Port { get; set; }
        public bool ? ConsoleMode { get; set; }

        public Settings Override(Settings other)
        {
            return new Settings()
            {
                Port = other.Port ?? Port,
                ConsoleMode = other.ConsoleMode ?? ConsoleMode,
            };
        }
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

            bool debugMode = false;

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
                case CommandNames.Install:
                    Install((InstallOptions)options);
                    break;

                case CommandNames.Uninstall:
                    Uninstall((UninstallOptions)options);
                    break;

                case CommandNames.Console:
                    RunConsole((ConsoleOptions)options);
                    break;
            }
        }

        private static void RunConsole(ConsoleOptions options)
        {
            throw new NotImplementedException();
        }

        private static void Uninstall(UninstallOptions options)
        {
            ManagedInstallerClass.InstallHelper(new[] {"/u", Assembly.GetExecutingAssembly().Location});
        }

        private static void Install(InstallOptions options)
        {
            if (IsServiceInstalled())
            {
                Uninstall(new UninstallOptions {Force = true});
            }

            ManagedInstallerClass.InstallHelper(new[] {Assembly.GetExecutingAssembly().Location});
        }

        protected override void OnStart(string[] args)
        {
            
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
    }
}