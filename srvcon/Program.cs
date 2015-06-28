﻿// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.ServiceProcess;
using System.Threading;
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

            var service = new Program();
            if (_consoleMode)
            {
                RunConsole(service);
            }
            else
            {
                Run(service);
            }
        }

        private static void Execute(string command, object options)
        {
            switch (command)
            {
                case CommandNames.Install:
                    InstallHelper.Install((InstallOptions) options);
                    return;

                case CommandNames.Uninstall:
                    InstallHelper.Uninstall((UninstallOptions) options);
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

        private static void RunConsole(Program service)
        {
            service.OnStart(null);

            var serviceStopped = Mutex.OpenExisting(RunningMutexName);
            var userCanceled = new ConsoleCtrlCEvent();
            WaitHandle.WaitAny(new WaitHandle[] {userCanceled, serviceStopped});

            service.OnStop();
        }
    }
}