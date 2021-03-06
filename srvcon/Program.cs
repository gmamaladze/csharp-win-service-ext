﻿// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.Log4net;
using Ninject.Modules;

namespace srvcon
{
    internal class Program : ServiceBase
    {
        public const string RunningMutexName = "srvcon.running";
        private static bool _consoleMode;
        private static Mutex _runningMutex;
        private readonly ILogger _log;
        private static StandardKernel _kernel;

        public Program(ILogger logger)
        {
            _log = logger;
            AutoLog = false;
        }

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                //TODO Send to log
                Console.Error.WriteLine(e.ToString());
            };
            var options = new Options();
            var isOk = Parser.Default.ParseArguments(args, options, Execute);
            if (!isOk) Environment.Exit(Parser.DefaultExitCodeFail);

            Logger.Init();

            //NOTE if you do not want implicit log extension initialization use this code
            //var setting = new NinjectSettings {LoadExtensions = false};
            //var module = new Log4NetModule();
            _kernel = new StandardKernel();

            _kernel.Bind<Program>().ToSelf();

            var service = _kernel.Get<Program>();
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
            _log.Info("Service started.");
        }

        protected override void OnStop()
        {
            _runningMutex.ReleaseMutex();
            _log.Info("Service stopped.");
        }

        private static void RunConsole(Program service)
        {
            service.OnStart(null);

            var userOrServiceStop = Task.Factory.StartNew(() =>
            {
                var serviceStopped = Mutex.OpenExisting(RunningMutexName);
                var userCanceled = new ConsoleCtrlCEvent(service._log);
                WaitHandle.WaitAny(new WaitHandle[] {userCanceled, serviceStopped});
            });

            userOrServiceStop.Wait();

            service.OnStop();
        }
    }
}