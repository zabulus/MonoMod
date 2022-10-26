﻿#nullable enable
using MonoMod.Logs;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// This class is included in every MonoMod assembly.
namespace MonoMod {
    internal static class MMDbgLog {
        public static bool IsWritingLog => DebugLog.IsWritingLog;

        [ModuleInitializer]
        [SuppressMessage("Usage", "CA2255:The 'ModuleInitializer' attribute should not be used in libraries",
            Justification = "We want to send a log message as early as possible to keep the log messages together")]
        internal static void LogVersion() {
            Info($"Version {AssemblyInfo.AssemblyVersion}");
        }

        public static void Log(LogLevel level, string message) {
            DebugLog.Log(AssemblyInfo.AssemblyName, level, message);
        }
        public static void Log(LogLevel level, ref DebugLogInterpolatedStringHandler message) {
            DebugLog.Log(AssemblyInfo.AssemblyName, level, ref message);
        }

        public static void Spam(string message) => Log(LogLevel.Spam, message);
        public static void Spam(ref DebugLogInterpolatedStringHandler message) => Log(LogLevel.Spam, ref message);

        public static void Trace(string message) => Log(LogLevel.Trace, message);
        public static void Trace(ref DebugLogInterpolatedStringHandler message) => Log(LogLevel.Trace, ref message);

        public static void Info(string message) => Log(LogLevel.Info, message);
        public static void Info(ref DebugLogInterpolatedStringHandler message) => Log(LogLevel.Info, ref message);

        public static void Warning(string message) => Log(LogLevel.Warning, message);
        public static void Warning(ref DebugLogInterpolatedStringHandler message) => Log(LogLevel.Warning, ref message);

        public static void Error(string message) => Log(LogLevel.Error, message);
        public static void Error(ref DebugLogInterpolatedStringHandler message) => Log(LogLevel.Error, ref message);
    }
}
