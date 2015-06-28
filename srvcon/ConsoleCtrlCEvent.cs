using System;
using System.Security.AccessControl;
using System.Threading;

namespace srvcon
{
    internal class ConsoleCtrlCEvent : EventWaitHandle
    {
        public ConsoleCtrlCEvent()
            : base(true, EventResetMode.AutoReset)
        {
            Console.WriteLine("Press Ctrl+C to exit...");
            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;
                Reset();
            };
        }


        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode)
            : base(initialState, mode)
        {
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode, string name) : base(initialState, mode, name)
        {
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode, string name, out bool createdNew)
            : base(initialState, mode, name, out createdNew)
        {
        }

        protected ConsoleCtrlCEvent(bool initialState, EventResetMode mode, string name, out bool createdNew, EventWaitHandleSecurity eventSecurity)
            : base(initialState, mode, name, out createdNew, eventSecurity)
        {
        }
    }
}