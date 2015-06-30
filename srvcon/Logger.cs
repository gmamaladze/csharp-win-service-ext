// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.Diagnostics;
using System.IO;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace srvcon
{
    internal class Logger
    {
        public static void Init()
        {
            var console = GetConsoleAppender();
            var eventLog = GetEventLogAppender();
            var fileLog = GetFileLogAppender();
            BasicConfigurator.Configure(console, eventLog, fileLog);
        }

        private static PatternLayout GetPattermLayout()
        {
            return new PatternLayout(
                "%level [%thread] %d{HH:mm:ss} - %message%newline"
                );
        }

        private static IAppender GetFileLogAppender()
        {
            var appender = new RollingFileAppender
            {
                Threshold = Level.All,
                StaticLogFileName = false,
                CountDirection = 0,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaximumFileSize = "1KB",
                MaxFileSize = 1*1024,
                MaxSizeRollBackups = 5,
                DatePattern = "yy.MM.dd.hh.mm.ss",
                Layout = GetPattermLayout(),
                ImmediateFlush = true,
                File = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    InstallHelper.InstallServiceName,
                    "service.log")
            };
            // Makes the logs count 1, 2, 3 1 KB
            appender.ActivateOptions();
            return appender;
        }

        private static IAppender GetEventLogAppender()
        {
            var appender = new EventLogAppender
            {
                Threshold = Level.Error,
                Layout = GetPattermLayout()
            };

            var mappers = new[]
            {
                new EventLogAppender.Level2EventLogEntryType
                {
                    EventLogEntryType = EventLogEntryType.Error,
                    Level = Level.Error
                },
                new EventLogAppender.Level2EventLogEntryType
                {
                    EventLogEntryType = EventLogEntryType.Warning,
                    Level = Level.Warn
                },
                new EventLogAppender.Level2EventLogEntryType
                {
                    EventLogEntryType = EventLogEntryType.Error,
                    Level = Level.Fatal
                }
            };

            foreach (var mapper in mappers)
            {
                appender.AddMapping(mapper);
            }
            appender.ActivateOptions();
            return appender;
        }

        private static IAppender GetConsoleAppender()
        {
            var appender = new ColoredConsoleAppender
            {
                Threshold = Level.All,
                Layout = GetPattermLayout()
            };

            var levelColors = new[]
            {
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Debug,
                    ForeColor = ColoredConsoleAppender.Colors.Cyan | ColoredConsoleAppender.Colors.HighIntensity
                },
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Info,
                    ForeColor = ColoredConsoleAppender.Colors.Green | ColoredConsoleAppender.Colors.HighIntensity
                },
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Warn,
                    ForeColor = ColoredConsoleAppender.Colors.Purple | ColoredConsoleAppender.Colors.HighIntensity
                },
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Error,
                    ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity
                },
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Fatal,
                    ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity,
                    BackColor = ColoredConsoleAppender.Colors.Red
                }
            };

            foreach (var levelColor in levelColors)
            {
                appender.AddMapping(levelColor);
            }
            appender.ActivateOptions();
            return appender;
        }
    }
}