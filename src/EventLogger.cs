namespace CustomSolutionTargets
{
    using System;
    using System.Diagnostics;

    internal static class EventLogger
    {
        internal static void Error(string message)
        {
            try
            {
                EventLog.WriteEntry("Microsoft Visual Studio", $"Custom Solution Targets Extension: {message ?? "null"}", EventLogEntryType.Error);
            }
            catch (Exception)
            {
                // Ignore
            }
        }
    }
}