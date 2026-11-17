namespace FederacionSantillan.App.Helpers;

public enum AccessibilityMode
{
    Standard,
    ColorblindSafe,
    HighContrast
}

public class ThemePalette
{
    public Color HeaderBack { get; init; }
    public Color SurfaceBack { get; init; }
    public Color PrimaryAction { get; init; }
    public Color DangerAction { get; init; }
    public Color OverdueCardBack { get; init; }
    public Color OverdueCardFore { get; init; }
    public Color UpcomingCardBack { get; init; }
    public Color UpcomingCardFore { get; init; }
    public Color TextPrimary { get; init; }
    public float BaseFontSize { get; init; }
}

public static class AccessibilityTheme
{
    public static AccessibilityMode CurrentMode { get; set; } = AccessibilityMode.Standard;
    public static bool LargeTextEnabled { get; set; } = false;

    public static ThemePalette GetCurrentPalette()
    {
        float fontSize = LargeTextEnabled ? 11F : 9.5F;

        return CurrentMode switch
        {
            // Paleta Okabe-Ito segura para daltonismo (Protanopia / Deuteranopia: Azul + Naranja/Ámbar)
            AccessibilityMode.ColorblindSafe => new ThemePalette
            {
                HeaderBack = Color.FromArgb(0, 68, 136),
                SurfaceBack = Color.FromArgb(248, 250, 252),
                PrimaryAction = Color.FromArgb(0, 114, 178),
                DangerAction = Color.FromArgb(213, 94, 0),
                OverdueCardBack = Color.FromArgb(255, 237, 213),
                OverdueCardFore = Color.FromArgb(154, 52, 18),
                UpcomingCardBack = Color.FromArgb(219, 234, 254),
                UpcomingCardFore = Color.FromArgb(30, 64, 175),
                TextPrimary = Color.FromArgb(15, 23, 42),
                BaseFontSize = fontSize
            },
            // Alto Contraste WCAG AAA para baja visión
            AccessibilityMode.HighContrast => new ThemePalette
            {
                HeaderBack = Color.Black,
                SurfaceBack = Color.White,
                PrimaryAction = Color.FromArgb(0, 51, 153),
                DangerAction = Color.FromArgb(153, 0, 0),
                OverdueCardBack = Color.FromArgb(255, 255, 204),
                OverdueCardFore = Color.Black,
                UpcomingCardBack = Color.FromArgb(224, 242, 254),
                UpcomingCardFore = Color.Black,
                TextPrimary = Color.Black,
                BaseFontSize = fontSize
            },
            _ => new ThemePalette
            {
                HeaderBack = Color.FromArgb(15, 76, 129),
                SurfaceBack = Color.FromArgb(248, 250, 252),
                PrimaryAction = Color.FromArgb(21, 128, 61),
                DangerAction = Color.FromArgb(185, 28, 28),
                OverdueCardBack = Color.FromArgb(254, 226, 226),
                OverdueCardFore = Color.FromArgb(153, 27, 27),
                UpcomingCardBack = Color.FromArgb(254, 243, 199),
                UpcomingCardFore = Color.FromArgb(146, 64, 14),
                TextPrimary = Color.FromArgb(15, 23, 42),
                BaseFontSize = fontSize
            }
        };
    }
}
