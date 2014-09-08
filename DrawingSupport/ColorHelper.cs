using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace DrawingSupport
{
    public static class ColorHelper
    {
        public static Color GenerateForeColor(int index, int count)
        {
            return ColorHelper.GenerateColor(index, count, 0.6f, 0.7f);
        }

        public static Color GenerateBackColor(int index, int count)
        {
            return ColorHelper.GenerateColor(index, count, 0.07f, 0.97f);
        }

        public static Color GenerateColor(int index, int count, float saturation = 0.8f, float brightness = 0.8f)
        {
            // einmal quer durch die Farbtöne im HSB-Farbraum
            float angle = ((index / (float)count) * 360) % 360;
            return ColorHelper.GetColorFromHSB(angle, saturation, brightness);
        }

        public static SolidColorBrush GenerateBrush(int index, int count, float saturation = 0.8f, float brightness = 0.8f)
        {
            return (SolidColorBrush)new SolidColorBrush(GenerateColor(index, count, saturation, brightness)).GetAsFrozen();
        }

        /// <summary>
        /// Erstellt eine System.Drawing.Color-Struktur aus den drei HSB-Komponenten (Farbton, Sättigung, Helligkeit)
        /// </summary>
        /// <param name="Hue">Farbton (0..360)</param>
        /// <param name="Saturation">Sättigung (0..1)</param>
        /// <param name="Brightness">Helligkeit (0..1)</param>
        /// <returns>eine System.Drawing.Color-Struktur</returns>
        public static Color GetColorFromHSB(float hue, float saturation, float brightness)
        {
            // Parameter prüfen
            if (hue < 0 || hue > 360)
                throw new ArgumentException("hue 0..360");
            if (saturation < 0 || saturation > 1)
                throw new ArgumentException("saturation 0..1");
            if (brightness < 0 || brightness > 1)
                throw new ArgumentException("brightness 0..1");

            if (saturation == 0)
            {
                // achromatische Farbe
                byte rgb = (byte)(brightness * 255);
                return Color.FromRgb(rgb, rgb, rgb);
            }
            else
            {
                float fHexHue = (6.0f / 360.0f) * hue;
                float fHexSector = (float)Math.Floor((double)fHexHue);
                float fHexSectorPos = fHexHue - fHexSector;

                float fBrightness = brightness * 255.0f;
                float fSaturation = saturation;

                byte bWashOut = (byte)(0.5f + fBrightness * (1.0f - fSaturation));
                byte bHueModifierOddSector = (byte)(0.5f + fBrightness * (1.0f - fSaturation * fHexSectorPos));
                byte bHueModifierEvenSector = (byte)(0.5f + fBrightness * (1.0f - fSaturation * (1.0f - fHexSectorPos)));

                // RGB-Farben abhängig vom Sektor erzeugen
                switch ((int)fHexSector)
                {
                    case 0:
                        return Color.FromRgb((byte)(brightness * 255), bHueModifierEvenSector, bWashOut);
                    case 1:
                        return Color.FromRgb(bHueModifierOddSector, (byte)(brightness * 255), bWashOut);
                    case 2:
                        return Color.FromRgb(bWashOut, (byte)(brightness * 255), bHueModifierEvenSector);
                    case 3:
                        return Color.FromRgb(bWashOut, bHueModifierOddSector, (byte)(brightness * 255));
                    case 4:
                        return Color.FromRgb(bHueModifierEvenSector, bWashOut, (byte)(brightness * 255));
                    case 5:
                        return Color.FromRgb((byte)(brightness * 255), bWashOut, bHueModifierOddSector);
                    default:
                        return Color.FromRgb(0, 0, 0);
                }
            }
        }
    }
}