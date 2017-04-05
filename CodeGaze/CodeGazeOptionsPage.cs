using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace CodeGaze
{
    [ClassInterface(ClassInterfaceType.AutoDual)]

    [ComVisible(true)]
    internal sealed class CodeGazeOptionsPage : DialogPage
    {
        private const string CategoryGeneral = "General";


        [Category(CategoryGeneral)]
        [DisplayName("Marker1 Text")]
        [Description("Text to display as Marker 1")]
        public string Marker1Text { get; set; }

        [Category(CategoryGeneral)]
        [DisplayName("Marker1 Indentation Level")]
        [Description("Level at which the marker starts to display")]
        public int Marker1IndentationLevel { get; set; }

        [Category(CategoryGeneral)]
        [DisplayName("Marker1 Color")]
        [Description("Foreground color of the first marker")]
        public Color Marker1Color { get; set; }


        [Category(CategoryGeneral)]
        [DisplayName("Marker2 Text")]
        [Description("Text to display as Marker 1")]
        public string Marker2Text { get; set; }

        [Category(CategoryGeneral)]
        [DisplayName("Marker2 Indentation Level")]
        [Description("Level at which the marker starts to display")]
        public int Marker2IndentationLevel { get; set; }

        [Category(CategoryGeneral)]
        [DisplayName("Marker2 Color")]
        [Description("Foreground color of the first marker")]
        public Color Marker2Color { get; set; }

        public override void LoadSettingsFromStorage()
        {
            this.Load();
            base.LoadSettingsFromStorage();
        }

        protected override void OnApply(DialogPage.PageApplyEventArgs e)
        {
            this.Apply();
        }

        private void Load()
        {
            CodeGazePackage.Settings.Read();

            this.Marker1Text = CodeGazePackage.Settings.Marker1Text;
            this.Marker1IndentationLevel = CodeGazePackage.Settings.Marker1IndentLevel;
            this.Marker1Color = CodeGazePackage.Settings.Marker1Color.ToDrawingColor();

            this.Marker2Text = CodeGazePackage.Settings.Marker2Text;
            this.Marker2IndentationLevel = CodeGazePackage.Settings.Marker2IndentLevel;
            this.Marker2Color = CodeGazePackage.Settings.Marker2Color.ToDrawingColor();
        }

        private void Apply()
        {
            CodeGazePackage.Settings.Marker1Text = this.Marker1Text;
            CodeGazePackage.Settings.Marker1IndentLevel = this.Marker1IndentationLevel;
            CodeGazePackage.Settings.Marker1Color = this.Marker1Color.ToMediaColor();

            CodeGazePackage.Settings.Marker2Text = this.Marker2Text;
            CodeGazePackage.Settings.Marker2IndentLevel = this.Marker2IndentationLevel;
            CodeGazePackage.Settings.Marker2Color = this.Marker2Color.ToMediaColor();

            CodeGazePackage.Settings.Write();
        }
    }
}
