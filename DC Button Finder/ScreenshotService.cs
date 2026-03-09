using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DC_Button_Finder
{
    public sealed class ScreenshotService
    {
        // Основная область с кнопками: X=420 до 820 (ширина 400px), полная высота экрана
        private readonly Rectangle _buttonsArea = new Rectangle(420, 0, 400, 720);

        // Область с "cross": диагональные углы (1220,0) и (1280,100) - ширина 60px, высота 100px
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
            // Для обратной совместимости
            return GetButtonsAreaScreenshot();
        }

        private Mat GetCleanScreenshot(Rectangle area, string areaName)
        {
            try
            {
                using (var bmp = new Bitmap(area.Width, area.Height))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(area.X, area.Y, 0, 0, area.Size);
                    }

                    // Конвертируем в Mat и сразу очищаем от метаданных
                    Mat source = BitmapConverter.ToMat(bmp);
                    Mat cleaned = CleanMatFromMetadata(source);
                    source.Dispose();

                    return cleaned;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка создания скриншота области {areaName}", ex);
            }
        }

        private Mat CleanMatFromMetadata(Mat source)
        {
            // Создаем полностью новую матрицу без метаданных
            Mat cleaned = new Mat();
            source.CopyTo(cleaned);

            // Принудительно устанавливаем тип и размеры
            if (cleaned.Depth() != MatType.CV_8U)
            {
                cleaned.ConvertTo(cleaned, MatType.CV_8U);
            }

            return cleaned;
        }

        // Методы для преобразования координат между областями
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