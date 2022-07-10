namespace CustomSolutionTargets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using EnvDTE80;
    using Microsoft.Build.Execution;
    using Microsoft.Build.Framework;
    using Microsoft.VisualStudio;

    internal class BuildService
    {
        private readonly DTE2 dte2;
        private readonly Logger logger;

        public BuildService(DTE2 dte2, Logger logger)
        {
            this.dte2 = dte2;
            this.logger = logger;
        }

        internal SolutionTargets GetTargetFiles()
        {
            var solutionFilePath = this.dte2.Solution.FullName;
            var solutionFolder = Path.GetDirectoryName(solutionFilePath);
            var solutionFileName = Path.GetFileName(solutionFilePath);
            var beforeTargetsFileName = $"before.{solutionFileName}.targets";
            var afterTargetsFileName = $"after.{solutionFileName}.targets";

            if (string.IsNullOrWhiteSpace(solutionFolder)) throw new InvalidOperationException("Solution not found");

            return new SolutionTargets
            {
                BeforeTargetsFilePath = Path.Combine(solutionFolder, beforeTargetsFileName),
                AfterTargetsFilePath = Path.Combine(solutionFolder, afterTargetsFileName)
            };
        }

        public int BuildTarget(string targetsFilePath, string activeBuildTarget)
        {
            var solutionConfigurationName = this.dte2.Solution.SolutionBuild.ActiveConfiguration.Name;

            var loggers = new List<ILogger> {this.logger};

            ProjectInstance solutionProjectInstance;
            try
            {
                solutionProjectInstance = new ProjectInstance(targetsFilePath);
            }
            catch (Exception ex)
            {
                this.logger.Write(LoggerVerbosity.Detailed, $"Custom Solution Targets Extension: failed to load target:{targetsFilePath}, Exception: {ex.Message}");
                return VSConstants.E_FAIL;
            }

            solutionProjectInstance.SetProperty("Configuration", solutionConfigurationName);
            solutionProjectInstance.SetProperty("BuildingInsideVisualStudio", "true");
            solutionProjectInstance.Build(activeBuildTarget, loggers);

            return VSConstants.S_OK;
        }
    }
}