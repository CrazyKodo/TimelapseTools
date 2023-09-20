using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePics
{
    public class RenameHelper
    {
        private static string _dateTimeStringPrefix = "yyyy-MM-dd_HHmmss";
        public static ProcessResult Rename(RenameType renameType, string sourcePath, string outputPath, bool replace)
        {
            if (renameType == RenameType.ExactDateTime)
            {
                DirectoryInfo d = new DirectoryInfo(sourcePath);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
                    var fileFullName = $"{outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}{f.Extension}";
                    Helper.TryCopy(f.FullName, fileFullName, replace);
                }
            }

            if (renameType == RenameType.ExactDateTime)
            {
                DirectoryInfo d = new DirectoryInfo(sourcePath);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
                    var fileFullName = $"{outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}_{f.Name}";
                    Helper.TryCopy(f.FullName, fileFullName, replace);
                }
            }

            if (renameType == RenameType.ExactDateTime)
            {
                DirectoryInfo d = new DirectoryInfo(sourcePath);
                FileInfo[] infos = d.GetFiles();
                var sortedInfo = infos.OrderBy(x => x.Name).ToList();

                for (int si = 0; si < sortedInfo.Count; si++)
                {
                    var extension = Path.GetExtension(sortedInfo[si].FullName);
                    var fileFullName = $"{outputPath}\\{(si + 1).ToString().PadLeft(5, '0')}{extension}";
                    Helper.TryCopy(sortedInfo[si].FullName, fileFullName, replace);
                }
            }
            return new ProcessResult() { };
        }

        private static DateTime TryGetDateTimeTakenFromExif(FileInfo fileInfo)
        {
            using (Image image = Image.FromFile(fileInfo.FullName))
            {
                if (!image.PropertyIdList.Any(x => x == 36867))
                {
                    return fileInfo.CreationTime;
                }
                PropertyItem propItem = image.GetPropertyItem(36867);
                string dateTaken = Encoding.UTF8.GetString(propItem.Value);
                string sdate = Encoding.UTF8.GetString(propItem.Value).Trim();
                string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
                string firsthalf = sdate.Substring(0, 10);
                firsthalf = firsthalf.Replace(":", "-");
                sdate = firsthalf + secondhalf;
                var dtaken = DateTime.Parse(sdate);

                return dtaken;
            }
        }
    }

    public enum RenameType
    {
        ExactDateTime,
        DateTimeWithFileName,
        IntByName
    }
}
