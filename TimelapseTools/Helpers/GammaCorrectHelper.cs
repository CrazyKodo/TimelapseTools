using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Reflection;

namespace MergePics
{
    public class GammaCorrectHelper
    {
        private static readonly Random rand = new Random();

        public static Image<Bgr, Byte> DrawSampleAreas(Image<Bgr, Byte> sample, int size, List<Point> points)
        {
            foreach (var point in points)
            {
                DrawSampleArea(sample, size, point);
            }

            return sample;
        }

        public static void GammaCorrect(string sourcePath, string outputPath, string gammaCorrectionSampleFile, bool replace, int threshold, int size, List<Point> points, BackgroundWorker backgroundWorker1)
        {
            DirectoryInfo d = new DirectoryInfo(sourcePath);
            FileInfo[] infos = d.GetFiles();

            var totalItems = infos.Length;
            var processed = 0m;

            using (Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(gammaCorrectionSampleFile))
            {
                Parallel.For(0, infos.Length, new ParallelOptions { MaxDegreeOfParallelism = 5 }, i =>
                {
                    using (Image<Bgr, Byte> img = new Image<Bgr, Byte>(infos[i].FullName))
                    {
                        var extension = Path.GetExtension(infos[i].FullName);
                        var fileFullName = $"{outputPath}\\{infos[i].Name.Replace(extension, "")}_GC{extension}";
                        if (!replace && File.Exists(fileFullName))
                        {
                            return;
                        }
                        try
                        {
                            var result = GammaCorrectHelper.GammaCorrect(sampleImg, img, threshold, size, points);
                            result.Save(fileFullName); 
                            processed++;
                            backgroundWorker1.ReportProgress(decimal.ToInt32(Math.Round(processed / totalItems * 100)));
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Message");
                        }

                    }
                });
            }
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
                    image._GammaCorrect(1.05d);
                }
                else
                {
                    image._GammaCorrect(0.95d);
                }
                imgAvgBrightness = GetImageAverageBrightness(image, size, points);
                diff = Math.Abs(sampleImgAvgBrightness - imgAvgBrightness);
            }

            return image;
        }

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

            points.ForEach(p => total += GetAverageBrightness(image, p.X, p.Y, size));

            return total / points.Count;
        }

        private static Image<Bgr, Byte> DrawSampleArea(Image<Bgr, Byte> sample, int size, Point point)
        {
            var seed = rand.Next(100, 400);
            try
            {
                for (int x = point.X; x < point.X + size; x++)
                {
                    for (int y = point.Y; y < point.Y + size; y++)
                    {
                        var currentBGR = sample[y, x];
                        var blue = 255 - currentBGR.Blue + (seed < 200 ? (seed - 100d) / 100 * 255 : 0);
                        var red = 255 - currentBGR.Red + ((200 <= seed && seed < 300) ? (seed - 200d) / 100 * 255 : 0);
                        var green = 255 - currentBGR.Green + (300 <= seed ? (seed - 300d) / 100 * 255 : 0);
                        var halfBGR = new Bgr(blue / 2, green / 2, red / 2);
                        sample[y, x] = halfBGR;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return sample;
        }

    }
}
