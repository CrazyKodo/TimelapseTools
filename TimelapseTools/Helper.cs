using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MergePics
{
    internal static class Helper
    {
        public static double GetBrightness(Bgr bgr)
        {
            return (0.2126 * bgr.Red + 0.7152 * bgr.Green + 0.0722 * bgr.Blue);
        }
        public static double GetAverageBrightness(IEnumerable<Bgr> bgrs)
        {
            int count = 0;
            double sumBrightness = 0;

            foreach (var color in bgrs)
            {
                count++;
                sumBrightness += GetBrightness(color);
            }

            return sumBrightness / count;
        }

        public static double GetAverageBrightness(Image<Bgr, Byte> image, int pointX, int pointY, int size)
        {
            var samples = new List<Bgr>();

            for (int x = pointX; x < pointX + size; x++)
            {
                for (int y = pointY; y < pointY + size; y++)
                {
                    samples.Add(image[y, x]);
                }
            }

            var sampleImgAvgBrightness = GetAverageBrightness(samples.ToList());

            return sampleImgAvgBrightness;
        }

        public static double GetImageAverageBrightness(Image<Bgr, Byte> image, int size, List<Point> points)
        {
            if (!points.Any())
            {
                return 0d;
            }

            var total = 0d;

            points.ForEach(p=> total+= GetAverageBrightness(image, p.X, p.Y, size) );

            return total/points.Count;
        }

        public static Image<Bgr, Byte> GammaCorrect(Image<Bgr, Byte> sample, Image<Bgr, Byte> image, int threshold, int size, List<Point> points)
        {
            var sampleImgAvgBrightness = GetImageAverageBrightness(sample, size, points);
            var imgAvgBrightness = GetImageAverageBrightness(image, size, points);
            var diff = Math.Abs(sampleImgAvgBrightness - imgAvgBrightness);

            while (diff >= threshold)
            {
                if (sampleImgAvgBrightness < imgAvgBrightness)
                {
                    image._GammaCorrect(1.1d);
                }
                else
                {
                    image._GammaCorrect(0.9d);
                }
                imgAvgBrightness = GetImageAverageBrightness(image, size, points);
                diff = Math.Abs(sampleImgAvgBrightness - imgAvgBrightness);
            }

            return image;
        }

        public static void SaveAppSettings(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                AppSettingsSection app = config.AppSettings;
                app.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception)
            {
            }
        }

        public static void TryCopy(string sourceFileName, string destFileName, bool replace)
        {
            if (!File.Exists(destFileName))
            {
                System.IO.File.Copy(sourceFileName, destFileName);
                return;
            }

            if (replace)
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return;
            }
        }
    }
}
