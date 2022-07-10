namespace CustomSolutionTargets
{
    using EnvDTE80;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Logging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    internal class Logger : ConsoleLogger
    {
        private LoggerVerbosity verbosity = LoggerVerbosity.Minimal;

        private Logger(IVsOutputWindowPane buildOutputWindowPane)
        {
            this.BuildOutputWindowPane = buildOutputWindowPane;
            this.WriteHandler = this.WriteToOutputWindowBuildPane;
            this.ShowSummary = false;
            this.SkipProjectStartedText = true;
            this.Verbosity = this.verbosity;
        }

        private IVsOutputWindowPane BuildOutputWindowPane { get; }

        public void ResetVerbosity(DTE2 dte2)
        {
            var properties = dte2.Properties["Environment", "ProjectsAndSolution"];
            this.verbosity = (LoggerVerbosity) properties.Item("MSBuildOutputVerbosity").Value;
        }

        public static Logger CreateInstance(IVsOutputWindow outputWindow, DTE2 dte2)
        {
            if (outputWindow is null)
            {
                EventLogger.Error("CustomSolutionTargets initialization failed. IVsOutputWindow could not be obtained.");
                return null;
            }

            var buildPaneGuid = VSConstants.GUID_BuildOutputWindowPane;

            var hResult = outputWindow.GetPane(ref buildPaneGuid, out var buildOutputWindowPane);
            if (hResult != VSConstants.S_OK || buildOutputWindowPane == null)
            {
                EventLogger.Error("CustomSolutionTargets initialization failed. IVsOutputWindow could not be obtained.");
                return null;
            }

            var instance = new Logger(buildOutputWindowPane);
            instance.ResetVerbosity(dte2);

            return instance;
        }

        private void WriteToOutputWindowBuildPane(string message)
        {
            this.BuildOutputWindowPane.OutputStringThreadSafe(message);
        }

        public void Write(LoggerVerbosity logVerbosity, string message)
        {
            if ((int) this.Verbosity < (int) logVerbosity)
            {
                return;
            }

            this.WriteToOutputWindowBuildPane(message);
        }
    }
}