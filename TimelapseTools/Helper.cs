using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    }
}
