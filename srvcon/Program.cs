// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace srvcon
{
    internal class Program : ServiceBase
    {
        public const string RunningMutexName = "srvcon.running";
        private static bool _consoleMode;
        private static Mutex _runningMutex;

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

        protected override void OnStart(string[] args)
        {
            _runningMutex = new Mutex(true, RunningMutexName);
            Console.WriteLine("Service started.");
        }

        protected override void OnStop()
        {
            _runningMutex.ReleaseMutex();
            Console.WriteLine("Service stopped.");
        }

        private static void RunConsole()
        {
            var service = new Program();
            service.OnStart(null);

            var serviceStopped = Mutex.OpenExisting(Program.RunningMutexName);
            var userCanceled = new ConsoleCtrlCEvent();
            WaitHandle.WaitAny(new WaitHandle[] { userCanceled, serviceStopped });

            service.OnStop();
        }
    }
}