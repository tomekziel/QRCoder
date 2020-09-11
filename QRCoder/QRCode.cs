#if NETFRAMEWORK || NETSTANDARD2_0
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using static QRCoder.QRCodeGenerator;

namespace QRCoder
{
    public class QRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public QRCode() { }

        public QRCode(QRCodeData data) : base(data) {}

        public Bitmap GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }

        public Bitmap GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true)
        {
            return this.GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones);
        }

        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            var bmp = new Bitmap(size, size);
            using (var gfx = Graphics.FromImage(bmp))
            using (var lightBrush = new SolidBrush(lightColor))
            using (var darkBrush = new SolidBrush(darkColor))
            {
                for (var x = 0; x < size + offset; x = x + pixelsPerModule)
                {
                    for (var y = 0; y < size + offset; y = y + pixelsPerModule)
                    {
                        var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];

                        if (module)
                        {
                            gfx.FillRectangle(darkBrush, new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                        }
                        else
                        {
                            gfx.FillRectangle(lightBrush, new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                        }
                    }
                }

                gfx.Save();
            }

            return bmp;
        }



        public Bitmap GetGraphic(int pixelsPerModule, ColorProvider provider, bool drawQuietZones = true, bool drawThinLines = false, bool drawDirections = false)
        {
            var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            var bmp = new Bitmap(size, size);

            using (var gfx = Graphics.FromImage(bmp))
            {
                for (var x = 0; x < size + offset; x = x + pixelsPerModule)
                {

                    for (var y = 0; y < size + offset; y = y + pixelsPerModule)
                    {
                        var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                        SourceType stype = (SourceType) this.QrCodeData.SourceMatrix[(y + pixelsPerModule) / pixelsPerModule - 1].GetValue((x + pixelsPerModule) / pixelsPerModule - 1);

                        var brush = provider.getBrush(stype, module);
                        gfx.FillRectangle(brush, new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));

                        
                    }
                }

                if (drawThinLines)
                {
                    for (var x = 0; x < size + offset; x = x + pixelsPerModule)
                    {
                        for (var y = 0; y < size + offset; y = y + pixelsPerModule)
                        {
                            gfx.DrawLine(Pens.LightGray, x, 0, x, size + offset);
                            gfx.DrawLine(Pens.LightGray, 0, y, size + offset, y);
                        }
                    }
                    gfx.DrawLine(Pens.LightGray, size -1, 0, size -1, size );
                    gfx.DrawLine(Pens.LightGray, 0, size -1, size , size -1);

                }

                if (drawDirections)
                {
                    for (int i = 1; i < this.QrCodeData.dataPoints.Count; i++)
                    {
                        var p1 = QrCodeData.dataPoints[i - 1];
                        var p2 = QrCodeData.dataPoints[i];

                        gfx.DrawLine(Pens.Black, 
                            p1.Item1*pixelsPerModule + pixelsPerModule / 2, p1.Item2*pixelsPerModule+ pixelsPerModule/2,
                            p2.Item1 * pixelsPerModule + pixelsPerModule / 2, p2.Item2 * pixelsPerModule + pixelsPerModule/2
                            );

                        gfx.FillRectangle(Brushes.Black,
                            new Rectangle(p1.Item1 * pixelsPerModule + pixelsPerModule / 2 - 1,
                            p1.Item2 * pixelsPerModule + pixelsPerModule / 2 - 1, 3, 3));
                        gfx.FillRectangle(Brushes.Black,
                            new Rectangle(p2.Item1 * pixelsPerModule + pixelsPerModule / 2 - 1,
                            p2.Item2 * pixelsPerModule + pixelsPerModule / 2 - 1, 3, 3));
                    }
                }
                gfx.Save();
            }

            return bmp;
        }



        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon=null, int iconSizePercent=15, int iconBorderWidth = 6, bool drawQuietZones = true)
        {
            var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            var bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (var gfx = Graphics.FromImage(bmp))
            using (var lightBrush = new SolidBrush(lightColor))
            using (var darkBrush = new SolidBrush(darkColor))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.Clear(lightColor);

                var drawIconFlag = icon != null && iconSizePercent > 0 && iconSizePercent <= 100;

                GraphicsPath iconPath = null;
                float iconDestWidth = 0, iconDestHeight = 0, iconX = 0, iconY = 0;

                if (drawIconFlag)
                {
                    iconDestWidth = iconSizePercent * bmp.Width / 100f;
                    iconDestHeight = drawIconFlag ? iconDestWidth * icon.Height / icon.Width : 0;
                    iconX = (bmp.Width - iconDestWidth) / 2;
                    iconY = (bmp.Height - iconDestHeight) / 2;

                    var centerDest = new RectangleF(iconX - iconBorderWidth, iconY - iconBorderWidth, iconDestWidth + iconBorderWidth * 2, iconDestHeight + iconBorderWidth * 2);
                    iconPath = this.CreateRoundedRectanglePath(centerDest, iconBorderWidth * 2);
                }

                for (var x = 0; x < size + offset; x = x + pixelsPerModule)
                {
                    for (var y = 0; y < size + offset; y = y + pixelsPerModule)
                    {
                        var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];

                        if (module)
                        {
                            var r = new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule);

                            if (drawIconFlag)
                            {
                                var region = new Region(r);
                                region.Exclude(iconPath);
                                gfx.FillRegion(darkBrush, region);
                            }
                            else
                            {
                                gfx.FillRectangle(darkBrush, r);
                            }
                        }
                        else
                        {
                            gfx.FillRectangle(lightBrush, new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                        }
                    }
                }

                if (drawIconFlag)
                {
                    var iconDestRect = new RectangleF(iconX, iconY, iconDestWidth, iconDestHeight);
                    gfx.DrawImage(icon, iconDestRect, new RectangleF(0, 0, icon.Width, icon.Height), GraphicsUnit.Pixel);
                }

                gfx.Save();
            }

            return bmp;
        }

        internal GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            var roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
    }

    public static class QRCodeHelper
    {
        public static Bitmap GetQRCode(string plainText, int pixelsPerModule, Color darkColor, Color lightColor, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, Bitmap icon = null, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new QRCode(qrCodeData))
                return qrCode.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones);
        }
    }
}

#endif