using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace CodeGaze
{
    public sealed class CodeGazeAdornment
    {
        private readonly IWpfTextView textView;
        private readonly IAdornmentLayer layer;
        private readonly IVsFontsAndColorsInformationService fontColorService;
        private readonly IVsEditorAdaptersFactoryService adaptersService;

        public CodeGazeAdornment(
            IWpfTextView textView,
            IVsFontsAndColorsInformationService fontColorService,
            IVsEditorAdaptersFactoryService adaptersService)
        {
            this.textView = textView;
            this.fontColorService = fontColorService;
            this.adaptersService = adaptersService;
            this.layer = this.textView.GetAdornmentLayer(nameof(CodeGazeAdornment));
            this.textView.LayoutChanged += HandleTextViewLayoutChanged;

        }

        private void HandleTextViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (var line in e.NewOrReformattedLines)
            {
                this.CreateVisuals(line);
            }
        }

        private void CreateVisuals(ITextViewLine line)
        {
            var g = this.textView.TextViewLines.GetMarkerGeometry(line.Extent);
            if (g == null) { return; }

            string t;
            if (TryGetText(this.textView, line, out t))
            {
                Color color;
                var marker = this.GetMarker(this.CalculateIndentationLevel(t), out color);

                if (marker != null)
                {
                    var adornment = new Border()
                    {
                        Height = g.Bounds.Height,
                        BorderThickness = new Thickness(1),
                    };
                    var text = new TextBlock()
                    {
                        Text = marker,
                        Background = this.textView.Background,
                        Foreground = new SolidColorBrush(color),
                        FontFamily = this.textView.FormattedLineSource.DefaultTextProperties.Typeface.FontFamily,
                        FontSize = g.Bounds.Height * 0.7,
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    adornment.Child = text;
                    Canvas.SetLeft(adornment, 10);
                    Canvas.SetTop(adornment, g.Bounds.Top);
                    this.layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, line.Extent, null, adornment, (tag, ui) => { });
                }
            }
        }

        /// <summary>
        /// This will get the text of the ITextView line as it appears in the actual user editable
        /// document.
        /// jared parson: https://gist.github.com/4320643
        /// </summary>
        private static bool TryGetText(IWpfTextView textView, ITextViewLine textViewLine, out string text)
        {
            var extent = textViewLine.Extent;
            var bufferGraph = textView.BufferGraph;
            try
            {
                var collection = bufferGraph.MapDownToSnapshot(extent, SpanTrackingMode.EdgeInclusive, textView.TextSnapshot);
                var span = new SnapshotSpan(collection[0].Start, collection[collection.Count - 1].End);
                //text = span.ToString();
                text = span.GetText();
                return true;
            }
            catch
            {
                text = null;
                return false;
            }
        }

        private string GetMarker(int indentation, out Color c)
        {
            if (indentation >= CodeGazePackage.Settings.Marker2IndentLevel)
            {
                c = CodeGazePackage.Settings.Marker2Color;
                return CodeGazePackage.Settings.Marker2Text;
            }
            else if (indentation >= CodeGazePackage.Settings.Marker1IndentLevel)
            {
                c = CodeGazePackage.Settings.Marker1Color;
                return CodeGazePackage.Settings.Marker1Text;
            }
            else
            {
                c = Color.FromArgb(0, 0, 0, 0);
                return null;
            }
        }

        private int CalculateIndentationLevel(string text)
        {
            int spaces = 0, tabs = 0, multiplier = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ') { spaces++; }
                else if (text[i] == '\t') { tabs++; }
                else { multiplier = 1; break; }
            }

            return (spaces / this.textView.FormattedLineSource.TabSize + tabs) * multiplier;
        }
    }
}
