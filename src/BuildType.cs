namespace CustomSolutionTargets
{
    using Microsoft.VisualStudio;

    public enum BuildType
    {
        /// <summary>
        ///     'build' action
        /// </summary>
        Build = VSConstants.VSStd97CmdID.BuildSln,

        /// <summary>
        ///     'rebuild' action
        /// </summary>
        Rebuild = VSConstants.VSStd97CmdID.RebuildSln,

        /// <summary>
        ///     'clean' action
        /// </summary>
        Clean = VSConstants.VSStd97CmdID.CleanSln,

        /// <summary>
        ///     'deploy' action
        /// </summary>
        Deploy = VSConstants.VSStd97CmdID.DeploySln,

        /// <summary>
        ///     'build' action for selection
        /// </summary>
        BuildSelection = VSConstants.VSStd97CmdID.BuildSel,

        /// <summary>
        ///     'rebuild' action for selection
        /// </summary>
        RebuildSelection = VSConstants.VSStd97CmdID.RebuildSel,

        /// <summary>
        ///     'clean' action for selection
        /// </summary>
        CleanSelection = VSConstants.VSStd97CmdID.CleanSel,

        /// <summary>
        ///     'deploy' action for selection
        /// </summary>
        DeploySelection = VSConstants.VSStd97CmdID.DeploySel,

        /// <summary>
        ///     'build' action for project
        /// </summary>
        BuildOnlyProject = VSConstants.VSStd2KCmdID.BuildOnlyProject,

        /// <summary>
        ///     'rebuild' action for project
        /// </summary>
        RebuildOnlyProject = VSConstants.VSStd2KCmdID.RebuildOnlyProject,

        /// <summary>
        ///     'clean' action for project
        /// </summary>
        CleanOnlyProject = VSConstants.VSStd2KCmdID.CleanOnlyProject,

        /// <summary>
        ///     'build' action for project
        /// </summary>
        BuildCtx = VSConstants.VSStd97CmdID.BuildCtx,

        /// <summary>
        ///     'rebuild' action for project
        /// </summary>
        RebuildCtx = VSConstants.VSStd97CmdID.RebuildCtx,

        /// <summary>
        ///     'clean' action for project
        /// </summary>
        CleanCtx = VSConstants.VSStd97CmdID.CleanCtx,

        /// <summary>
        ///     'deploy' action for project
        /// </summary>
        DeployCtx = VSConstants.VSStd97CmdID.DeployCtx
    }
}