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

        public static int MakeGrayscale(Color color)
        {
            //create the grayscale version of the pixel
            int grayScale = (int)((color.R * .3) + (color.G * .59) + (color.B * .11));

            return grayScale;
        }
    }
}
