// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using CommandLine;

namespace srvcon
{
    internal class UninstallOptions
    {
        [Option('f', "force")]
        public bool Force { get; set; }
    }
}