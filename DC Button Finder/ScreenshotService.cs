using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DC_Button_Finder
{
    public sealed class ScreenshotService
    {
        private readonly Rectangle _buttonsArea = new Rectangle(420, 0, 400, 720);
        private readonly Rectangle _crossArea = new Rectangle(1220, 0, 60, 100);

        public Mat GetButtonsAreaScreenshot()
        {
            return GetCleanScreenshot(_buttonsArea, "buttons_area");
        }

        public Mat GetCrossAreaScreenshot()
        {
            return GetCleanScreenshot(_crossArea, "cross_area");
        }

        public Mat GetScreenshot()
        {
            return GetButtonsAreaScreenshot();
        }

        private Mat GetCleanScreenshot(Rectangle area, string areaName)
        {
            try
            {
                using (var bmp = new Bitmap(area.Width, area.Height, PixelFormat.Format32bppArgb))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.CopyFromScreen(area.X, area.Y, 0, 0, area.Size);
                    }

                    Mat source = BitmapConverter.ToMat(bmp);
                    Mat bgrImage = new Mat();
                    Cv2.CvtColor(source, bgrImage, ColorConversionCodes.BGRA2BGR);
                    source.Dispose();

                    return bgrImage;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка создания скриншота области {areaName}", ex);
            }
        }

        public System.Drawing.Point ConvertToButtonsAreaCoords(System.Drawing.Point screenPoint)
        {
            return new System.Drawing.Point(screenPoint.X - _buttonsArea.X, screenPoint.Y - _buttonsArea.Y);
        }

        public System.Drawing.Point ConvertToCrossAreaCoords(System.Drawing.Point screenPoint)
        {
            return new System.Drawing.Point(screenPoint.X - _crossArea.X, screenPoint.Y - _crossArea.Y);
        }

        public System.Drawing.Point ConvertFromButtonsAreaCoords(System.Drawing.Point areaPoint)
        {
            return new System.Drawing.Point(areaPoint.X + _buttonsArea.X, areaPoint.Y + _buttonsArea.Y);
        }

        public System.Drawing.Point ConvertFromCrossAreaCoords(System.Drawing.Point areaPoint)
        {
            return new System.Drawing.Point(areaPoint.X + _crossArea.X, areaPoint.Y + _crossArea.Y);
        }

        public Rectangle GetButtonsArea() => _buttonsArea;
        public Rectangle GetCrossArea() => _crossArea;
    }
}