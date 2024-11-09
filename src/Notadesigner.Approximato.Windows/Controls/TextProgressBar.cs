using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Notadesigner.Approximato.Windows.Controls;

public class TextProgressBar : ProgressBar
{
    private TextProgressBarDisplayMode _visualMode = TextProgressBarDisplayMode.Progress;

    private SolidBrush _textColourBrush = (SolidBrush)Brushes.Black;

    public TextProgressBar()
    {
        Value = Minimum;
        FixComponentBlinking();
    }

    [Category("Additional Options")]
    public Color TextColor
    {
        get => _textColourBrush.Color;

        set
        {
            _textColourBrush.Dispose();
            _textColourBrush = new SolidBrush(value);
        }
    }

    [Category("Additional Options"), Browsable(true)]
    public TextProgressBarDisplayMode DisplayMode
    {
        get
        {
            return _visualMode;
        }
        set
        {
            _visualMode = value;
            // Redraw after value is changed in the Properties window
            Invalidate();
        }
    }

    [Description("If it's empty, % will be shown"), Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
    [AllowNull]
    public override string Text
    {
        get => base.Text;

        set
        {
            base.Text = value;
            // Redraw after value is changed in the Properties window
            Invalidate();
        }
    }

    private string GetTextToDraw()
    {
        string text = Text;

        switch (DisplayMode)
        {
            case (TextProgressBarDisplayMode.Percentage):
                text = GetPercentageString();
                break;

            case (TextProgressBarDisplayMode.Progress):
                text = GetCurrentProgressString();
                break;

            case (TextProgressBarDisplayMode.TextAndProgress):
                text = $"{Text}: {GetCurrentProgressString()}";
                break;

            case (TextProgressBarDisplayMode.TextAndPercentage):
                text = $"{Text}: {GetPercentageString()}";
                break;
        }

        return text;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        DrawProgressBar(g);

        DrawStringIfNeeded(g);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _textColourBrush.Dispose();
        }

        base.Dispose(disposing);
    }

    private string GetPercentageString() => $"{(int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100} %";

    private string GetCurrentProgressString() => $"{Value}/{Maximum}";

    private void FixComponentBlinking()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    private void DrawProgressBar(Graphics g)
    {
        Rectangle rect = ClientRectangle;

        ProgressBarRenderer.DrawHorizontalBar(g, rect);

        rect.Inflate(-3, -3);

        if (Value > 0)
        {
            var clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);

            g.FillRectangle(new SolidBrush(ForeColor), clip);
        }
    }

    private void DrawStringIfNeeded(Graphics g)
    {
        if (DisplayMode != TextProgressBarDisplayMode.NoText)
        {
            var text = GetTextToDraw();
            var len = g.MeasureString(text, Font);
            var location = new Point(((Width / 2) - (int)len.Width / 2), ((Height / 2) - (int)len.Height / 2));
            g.DrawString(text, Font, _textColourBrush, location);
        }
    }
}