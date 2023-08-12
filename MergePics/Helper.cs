using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    }
}
