using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MergePics
{
    public class RotateHelper
    {
        public static ProcessResult Rotate(RotateFlipType rotateFlipType, string sourcePath, string outputPath, BackgroundWorker backgroundWorker1)
        {
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            ImageCodecInfo jgpEncoder = codecs.First(x => x.FormatID == ImageFormat.Jpeg.Guid);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            DirectoryInfo d = new DirectoryInfo(sourcePath);
            FileInfo[] infos = d.GetFiles();

            var totalItems = infos.Length;
            var processed = 0m;

            Parallel.For(0, infos.Length, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                var f = infos[i];
                var fileFullName = $"{outputPath}\\{f.Name}";
                using (Image img = Image.FromFile(f.FullName))
                {
                    //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                    img.RotateFlip(rotateFlipType);
                    img.Save(fileFullName, jgpEncoder, myEncoderParameters);
                }
                processed++;
                backgroundWorker1.ReportProgress(decimal.ToInt32(Math.Round(processed / totalItems * 100)));
            });

            return new ProcessResult() { };
        }
    }
}
