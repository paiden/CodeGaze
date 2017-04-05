using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodeGaze
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(PackageGuids.CodeGazePackageGuid)]
    [ProvideAutoLoad(UIContextGuids.CodeWindow)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideOptionPage(typeof(CodeGazeOptionsPage), "CodeGaze", "General", 0, 0, true)]
    public sealed class CodeGazePackage : Package
    {
        private const string PackageName = "CodeGaze";

        internal static CodeGazeSettings Settings { get; private set; }

        public CodeGazePackage()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PackageName);
            path = Path.Combine(path, "settings.ini");
            Settings = new CodeGazeSettings(path);
            Settings.Read();
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
