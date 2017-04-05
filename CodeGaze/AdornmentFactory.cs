using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace CodeGaze
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class AdornmentFactory : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
            => new CodeGazeAdornment(textView, this.fontColorService, this.adaptersService);

        [Export(typeof(AdornmentLayerDefinition))]
        [Name(nameof(CodeGazeAdornment))]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        [Import]

        IVsFontsAndColorsInformationService fontColorService = null;

        [Import]
        private readonly IVsEditorAdaptersFactoryService adaptersService;
    }
}
