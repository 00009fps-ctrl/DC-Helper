using OpenCvSharp;
using System;

namespace DC_Button_Finder
{
    public sealed class TemplateMatcher
    {
        private readonly Logger _logger;

        public TemplateMatcher(Logger logger)
        {
            _logger = logger;
        }

        public MatchResult FindTemplate(Mat screenshot, Mat template, string templateName, double threshold)
        {
            try
            {
                Mat processedScreenshot = new Mat();
                Mat processedTemplate = new Mat();

                if (screenshot.Channels() == 4)
                    Cv2.CvtColor(screenshot, processedScreenshot, ColorConversionCodes.BGRA2BGR);
                else if (screenshot.Channels() == 1)
                    Cv2.CvtColor(screenshot, processedScreenshot, ColorConversionCodes.GRAY2BGR);
                else
                    screenshot.CopyTo(processedScreenshot);

                if (template.Channels() == 4)
                    Cv2.CvtColor(template, processedTemplate, ColorConversionCodes.BGRA2BGR);
                else if (template.Channels() == 1)
                    Cv2.CvtColor(template, processedTemplate, ColorConversionCodes.GRAY2BGR);
                else
                    template.CopyTo(processedTemplate);

                if (processedScreenshot.Depth() != MatType.CV_8U)
                    processedScreenshot.ConvertTo(processedScreenshot, MatType.CV_8U);

                if (processedTemplate.Depth() != MatType.CV_8U)
                    processedTemplate.ConvertTo(processedTemplate, MatType.CV_8U);

                using (Mat result = new Mat())
                {
                    Cv2.MatchTemplate(processedScreenshot, processedTemplate, result, TemplateMatchModes.CCoeffNormed);
                    Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                    double confidence = maxVal;
                    double confidencePercent = confidence * 100.0;

                    var matchResult = new MatchResult
                    {
                        Found = confidence >= threshold,
                        Location = maxLoc,
                        Confidence = confidence,
                        ConfidencePercent = confidencePercent,
                        TemplateSize = new System.Drawing.Size(template.Width, template.Height),
                        TemplateName = templateName
                    };

                    if (matchResult.Found)
                        _logger.Info($"Найдено: {templateName} - {confidencePercent:F1}% (порог: {threshold * 100:F1}%)");
                    else
                        _logger.Debug($"Не найдено: {templateName} - {confidencePercent:F1}% (порог: {threshold * 100:F1}%)");

                    processedScreenshot.Dispose();
                    processedTemplate.Dispose();

                    return matchResult;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка поиска шаблона {templateName}: {ex.Message}");
                return MatchResult.NotFound(templateName);
            }
        }
    }

    public struct MatchResult
    {
        public bool Found { get; set; }
        public OpenCvSharp.Point Location { get; set; }
        public double Confidence { get; set; }
        public double ConfidencePercent { get; set; }
        public System.Drawing.Size TemplateSize { get; set; }
        public string TemplateName { get; set; }

        public static MatchResult NotFound(string templateName)
        {
            return new MatchResult { Found = false, TemplateName = templateName };
        }

        public System.Drawing.Point GetRandomPointInTemplate(Random random)
        {
            if (!Found) return System.Drawing.Point.Empty;

            int minX = Location.X + TemplateSize.Width / 4;
            int maxX = Location.X + TemplateSize.Width * 3 / 4;
            int minY = Location.Y + TemplateSize.Height / 4;
            int maxY = Location.Y + TemplateSize.Height * 3 / 4;

            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);

            return new System.Drawing.Point(x, y);
        }

        public System.Drawing.Point GetRandomPointInBottomRightCorner(Random random)
        {
            if (!Found) return System.Drawing.Point.Empty;

            int maxX = Location.X + TemplateSize.Width;
            int minX = maxX - 40;
            int maxY = Location.Y + TemplateSize.Height;
            int minY = maxY - 5;
            int actualMaxY = maxY + 5;

            int x = random.Next(minX, maxX + 1);
            int y = random.Next(minY, actualMaxY + 1);

            return new System.Drawing.Point(x, y);
        }

        public System.Drawing.Rectangle GetBoundingBox()
        {
            return new System.Drawing.Rectangle(Location.X, Location.Y, TemplateSize.Width, TemplateSize.Height);
        }
    }
}