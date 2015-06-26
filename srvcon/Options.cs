// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using CommandLine;

namespace srvcon
{
    internal class Options
    {
        [VerbOption(CommandNames.Install, HelpText = "Install the service.")]
        public InstallOptions Install { get; set; }

        [VerbOption(CommandNames.Uninstall, HelpText = "Uninstall the service.")]
        public UninstallOptions Uninstall { get; set; }

        [VerbOption(CommandNames.Console, HelpText = "Start service as a console application.")]
        public ConsoleOptions Console { get; set; }
    }
}