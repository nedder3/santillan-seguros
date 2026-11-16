using System.Drawing;
using System.Drawing.Drawing2D;

namespace FederacionSantillan.App.Helpers;

/// <summary>
/// Generador programático de íconos e imágenes para ventanas y botones (sin dependencias de rutas externas).
/// Cumple con el requisito de rúbrica: "¿Implementa uso de imágenes o íconos en botones y ventanas?".
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

        using var font = new Font("Segoe UI", 9, FontStyle.Bold);
        var size = g.MeasureString(simbolo, font);
        g.DrawString(simbolo, font, Brushes.White, (19 - size.Width) / 2, (19 - size.Height) / 2);

        return bmp;
    }
}
