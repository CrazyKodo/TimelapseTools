using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePics
{
    internal class RenameHelper
    {
        public static ProcessResult Rename()
        {
            //if (cbFileNamePrefix.SelectedItem == "ExactDateTime")
            //{
            //    DirectoryInfo d = new DirectoryInfo(_sourcePath);
            //    FileInfo[] infos = d.GetFiles();
            //    foreach (FileInfo f in infos)
            //    {
            //        var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
            //        var fileFullName = $"{_outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}{f.Extension}";
            //        Helper.TryCopy(f.FullName, fileFullName, cbRenameReplace.Checked);
            //    }
            //}

            //if (cbFileNamePrefix.SelectedItem == "DateTimeWithFileName")
            //{
            //    DirectoryInfo d = new DirectoryInfo(_sourcePath);
            //    FileInfo[] infos = d.GetFiles();
            //    foreach (FileInfo f in infos)
            //    {
            //        var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
            //        var fileFullName = $"{_outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}_{f.Name}";
            //        Helper.TryCopy(f.FullName, fileFullName, cbRenameReplace.Checked);
            //    }
            //}

            //if (cbFileNamePrefix.SelectedItem == "IntByName")
            //{
            //    DirectoryInfo d = new DirectoryInfo(_sourcePath);
            //    FileInfo[] infos = d.GetFiles();
            //    var sortedInfo = infos.OrderBy(x => x.Name).ToList();

            //    for (int si = 0; si < sortedInfo.Count; si++)
            //    {
            //        var extension = Path.GetExtension(sortedInfo[si].FullName);
            //        var fileFullName = $"{_outputPath}\\{(si + 1).ToString().PadLeft(5, '0')}{extension}";
            //        Helper.TryCopy(sortedInfo[si].FullName, fileFullName, cbRenameReplace.Checked);
            //    }
            //}
            return new ProcessResult() { };
        }
    }
}
