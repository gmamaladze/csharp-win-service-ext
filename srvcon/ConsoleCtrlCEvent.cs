// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.Security.AccessControl;
using System.Threading;
using Ninject.Extensions.Logging;

namespace srvcon
{
    internal class ConsoleCtrlCEvent : EventWaitHandle
    {
        public ConsoleCtrlCEvent(ILogger log)
            : base(false, EventResetMode.ManualReset)
        {
            Console.WriteLine("Press Ctrl+C to exit...");
            Console.CancelKeyPress += (sender, args) =>
            {
                log.Info("Exit requested by user.");
                args.Cancel = true;
                Set();
            };
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode)
            : base(initialState, mode)
        {
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode, string name)
            : base(initialState, mode, name)
        {
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode, string name, out bool createdNew)
            : base(initialState, mode, name, out createdNew)
        {
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode, string name, out bool createdNew,
            EventWaitHandleSecurity eventSecurity)
            : base(initialState, mode, name, out createdNew, eventSecurity)
        {
        }
    }
}