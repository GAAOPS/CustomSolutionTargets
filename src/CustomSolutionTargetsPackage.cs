namespace CustomSolutionTargets
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using EnvDTE80;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [Guid(Vsix.Guid)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class CustomSolutionTargetsPackage : AsyncPackage, IVsUpdateSolutionEvents
    {
        private BuildService buildService;
        private CommandEventsWrapper commandEvents;
        private DTE2 dte2;
        private Logger logger;
        private IVsSolutionBuildManager2 solutionBuildManager;
        private uint updateSolutionEventsCookie;

        public int UpdateSolution_Begin(ref int pfCancelUpdate) => 0;

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            SolutionTargets targets;

            try
            {
                targets = this.buildService.GetTargetFiles();
            }
            catch (InvalidOperationException)
            {
                return VSConstants.E_UNEXPECTED;
            }


            if (!File.Exists(targets.AfterTargetsFilePath))
            {
                return VSConstants.S_OK;
            }

            var result = this.buildService.BuildTarget(targets.AfterTargetsFilePath, this.commandEvents.ActiveBuildTarget);

            this.commandEvents.ResetActiveTarget();

            return result;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            SolutionTargets targets;

            try
            {
                targets = this.buildService.GetTargetFiles();
            }
            catch (InvalidOperationException)
            {
                return VSConstants.E_UNEXPECTED;
            }


            if (!File.Exists(targets.BeforeTargetsFilePath))
            {
                return VSConstants.S_OK;
            }

            return this.buildService.BuildTarget(targets.BeforeTargetsFilePath, this.commandEvents.ActiveBuildTarget);
        }

        public int UpdateSolution_Cancel() => 0;

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy) => 0;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.solutionBuildManager != null && this.updateSolutionEventsCookie != 0)
            {
                if (ThreadHelper.CheckAccess())
                {
                    this.solutionBuildManager.UnadviseUpdateSolutionEvents(this.updateSolutionEventsCookie);
                }
            }
        }


        /// <summary>
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token to monitor for initialization cancellation, which can occur when
        ///     VS is shutting down.
        /// </param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>
        ///     A task representing the async work of package initialization, or an already completed task if there is none.
        ///     Do not return null from this method.
        /// </returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            this.dte2 = await this.GetServiceAsync(typeof(SDTE)) as DTE2;

            if (this.dte2 == null)
            {
                EventLogger.Error("CustomSolutionTargets initialization failed. DTE2 could not be obtained.");
                return;
            }

            this.solutionBuildManager =
                ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;

            this.solutionBuildManager?.AdviseUpdateSolutionEvents(this, out this.updateSolutionEventsCookie);

            var outputWindow = await this.GetServiceAsync(typeof(SVsOutputWindow)) as IVsOutputWindow;

            this.logger = Logger.CreateInstance(outputWindow, this.dte2);

            if (this.logger is null)
            {
                return;
            }

            this.buildService = new BuildService(this.dte2, this.logger);
            this.commandEvents = CommandEventsWrapper.CreateInstance(this.dte2);
        }
    }
}