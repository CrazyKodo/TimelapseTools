using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace MergePics
{
    public class MidFrameHelper
    {
        public static void CreateFrame(string sourcePath, string outputPath, bool replace, BackgroundWorker backgroundWorker1)
        {
            DirectoryInfo d = new DirectoryInfo(sourcePath);
            FileInfo[] infos = d.GetFiles();

            var totalItems = infos.Length;
            var processed = 0m;

            Parallel.For(0, infos.Length - 1, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                using (var img = Image.FromFile(infos[i].FullName))
                using (var img1 = Image.FromFile(infos[i + 1].FullName))
                {
                    var extension = Path.GetExtension(infos[i].FullName);
                    var fileFullName = $"{outputPath}\\{infos[i].Name.Replace(extension, "")}_Mid{extension}";
                    if (!replace && File.Exists(fileFullName))
                    {
                        return;
                    }
                    try
                    {
                        using (var result = new Bitmap(img.Width, img.Height))
                        {
                            var results = MergeImage((Bitmap)img, (Bitmap)img1, result);
                            if (results != null)
                            {
                                results.Save(fileFullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
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

        private static Bitmap MergeImage(Bitmap image1, Bitmap image2, Bitmap bitmap)
        {
            bitmap.SetResolution(image1.VerticalResolution, image1.HorizontalResolution);
            for (int y = 0; y < image1.Height; y++)
            {
                for (int x = 0; x < image1.Width; x++)
                {
                    var pix1 = image1.GetPixel(x, y);
                    var pix2 = image2.GetPixel(x, y);
                    if (pix1 != pix2)
                    {
                        var midPix = MergeColor(pix1, pix2);
                        bitmap.SetPixel(x, y, midPix);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, pix1);
                    }
                }
            }

            return bitmap;
        }

        private static Color MergeColor(Color color1, Color color2)
        {
            var midR = ((int)color1.R + (int)color2.R) / 2;
            var midG = ((int)color1.G + (int)color2.G) / 2;
            var midB = ((int)color1.B + (int)color2.B) / 2;

            return Color.FromArgb(midR, midG, midB);
        }

    }
}
