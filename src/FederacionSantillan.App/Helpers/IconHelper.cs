using System.Drawing;
using System.Drawing.Drawing2D;

namespace FederacionSantillan.App.Helpers;

/// <summary>
/// Generador programático de íconos vectoriales GDI+ para ventanas y botones (sin dependencias de rutas externas ni emojis).
/// </summary>
public static class IconHelper
{
    public static Icon CrearIconoEscudo()
    {
        using var bmp = new Bitmap(32, 32);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);

        using var brush = new SolidBrush(Color.FromArgb(15, 76, 129));
        using var pen = new Pen(Color.FromArgb(212, 175, 55), 2f);
        Point[] shield = {
            new(16, 2), new(28, 7), new(26, 20), new(16, 30), new(6, 20), new(4, 7)
        };
        g.FillPolygon(brush, shield);
        g.DrawPolygon(pen, shield);

        using var font = new Font("Segoe UI", 11, FontStyle.Bold);
        g.DrawString("S", font, Brushes.White, 9, 6);

        return Icon.FromHandle(bmp.GetHicon());
    }

    public static Image CrearIconoCirculo(Color color, string simbolo)
    {
        var bmp = new Bitmap(20, 20);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);

        using var brush = new SolidBrush(color);
        g.FillEllipse(brush, 1, 1, 17, 17);

        using var pen = new Pen(Color.White, 1.9f)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };

        switch (simbolo)
        {
            case "check":
            case "✓":
                g.DrawLines(pen, new PointF[] { new(5.5f, 10f), new(8.5f, 13f), new(14f, 6.5f) });
                break;
            case "plus":
            case "+":
                g.DrawLine(pen, 9.5f, 5.5f, 9.5f, 13.5f);
                g.DrawLine(pen, 5.5f, 9.5f, 13.5f, 9.5f);
                break;
            case "close":
            case "✕":
                g.DrawLine(pen, 6.5f, 6.5f, 12.5f, 12.5f);
                g.DrawLine(pen, 12.5f, 6.5f, 6.5f, 12.5f);
                break;
            case "back":
            case "←":
                g.DrawLine(pen, 13.5f, 9.5f, 6f, 9.5f);
                g.DrawLines(pen, new PointF[] { new(9f, 6.5f), new(6f, 9.5f), new(9f, 12.5f) });
                break;
            case "refresh":
            case "↻":
                g.DrawArc(pen, 5.5f, 5.5f, 8f, 8f, -45f, 270f);
                g.DrawLines(pen, new PointF[] { new(11.5f, 4.5f), new(13.5f, 6.5f), new(11f, 8f) });
                break;
            default:
                using (var font = new Font("Segoe UI", 9, FontStyle.Bold))
                {
                    var size = g.MeasureString(simbolo, font);
                    g.DrawString(simbolo, font, Brushes.White, (19 - size.Width) / 2, (19 - size.Height) / 2);
                }
                break;
        }

        return bmp;
    }
}
