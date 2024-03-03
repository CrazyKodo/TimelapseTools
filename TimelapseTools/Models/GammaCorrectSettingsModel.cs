using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePics.Models
{
    internal class GammaCorrectSettingsModel
    {
        public int SampleSizePX { get; set; }
        public int Threshold { get; set; }
        public string SampleFilePath { get;set; }
        public int SamplePointsCount { get;set; }
        public List<Point> SamplePoints {get;set;} = new List<Point>();
    }
}
