// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.ServiceProcess;
using CommandLine;

namespace srvcon
{
    internal class Program : ServiceBase
    {
        private static bool _consoleMode;

        private static void Main(string[] args)
        {
            var options = new Options();
            var isOk = Parser.Default.ParseArguments(args, options, Execute);
            if (!isOk) Environment.Exit(Parser.DefaultExitCodeFail);

            if (_consoleMode)
            {
                RunConsole();
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
                    InstallHelper.Install((InstallOptions) options);
                    Environment.Exit(0);
                    return;

                case CommandNames.Uninstall:
                    InstallHelper.Uninstall((UninstallOptions) options);
                    Environment.Exit(0);
                    return;

                case CommandNames.Console:
                    _consoleMode = true;
                    break;
            }
        }

        private static void RunConsole()
        {
            Program service = new Program();
            service.OnStart(null);
            Console.WriteLine("Service Started...");
            Console.WriteLine("<press any key to exit...>");
            Console.Read();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}