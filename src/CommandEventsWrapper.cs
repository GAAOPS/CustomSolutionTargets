namespace CustomSolutionTargets
{
    using System;
    using System.Runtime.InteropServices;
    using EnvDTE80;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;

    public class CommandEventsWrapper
    {
        public string ActiveBuildTarget { get; private set; }

        public string CommandGuid { get; set; }

        public static CommandEventsWrapper CreateInstance(DTE2 dte2)
        {
            var vsStd2KCmdIdGuidAttribute =
                typeof(VSConstants.VSStd2KCmdID).GetCustomAttributes(typeof(GuidAttribute), true)[0] as GuidAttribute;

            var instance = new CommandEventsWrapper
            {
                CommandGuid = $"{{{vsStd2KCmdIdGuidAttribute.Value}}}"
            };

            ThreadHelper.ThrowIfNotOnUIThread();

            dte2.Events.CommandEvents.BeforeExecute += instance.CommandEvents_BeforeExecute;

            return instance;
        }

        public void ResetActiveTarget()
        {
            this.ActiveBuildTarget = null;
        }

        private void CommandEvents_BeforeExecute(string guid,
            int id,
            object customIn,
            object customOut,
            ref bool cancelDefault)
        {
            if (!guid.Equals(this.CommandGuid, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            switch (id)
            {
                case (int) BuildType.Build:
                case (int) BuildType.BuildSelection:
                case (int) BuildType.BuildOnlyProject:
                case (int) BuildType.BuildCtx:
                    this.ActiveBuildTarget = "Build";
                    break;
                case (int) BuildType.Rebuild:
                case (int) BuildType.RebuildSelection:
                case (int) BuildType.RebuildOnlyProject:
                case (int) BuildType.RebuildCtx:
                    this.ActiveBuildTarget = "Rebuild";
                    break;
                case (int) BuildType.Clean:
                case (int) BuildType.CleanSelection:
                case (int) BuildType.CleanOnlyProject:
                case (int) BuildType.CleanCtx:
                    this.ActiveBuildTarget = "Clean";
                    break;
                case (int) BuildType.Deploy:
                case (int) BuildType.DeploySelection:
                case (int) BuildType.DeployCtx:
                    this.ActiveBuildTarget = "Deploy";
                    break;
            }
        }
    }
}