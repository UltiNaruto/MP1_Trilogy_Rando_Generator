using Nanook.NKit;
using System.IO;
using System.Text.RegularExpressions;

namespace MP1_Trilogy_Rando_Generator
{
    class NKitManager
    {
        static bool IsValidFilename(string value)
        {
            return Regex.IsMatch(value, @"^[\w\-. ]+$");
        }

        public static bool ExtractISO(string filename)
        {
            bool result = true;
            if (!Directory.Exists(@".\tmp\nkit"))
                Directory.CreateDirectory(@".\tmp\nkit");
            try
            {
                using(var ndisc = new NDisc(null, File.OpenRead(filename)))
                {
                    if (ndisc == null)
                        throw new System.Exception();
                    if (ndisc.ExtractBasicInfo().Id.Substring(0, 6) != "R3ME01" &&
                        ndisc.ExtractBasicInfo().Id.Substring(0, 6) != "R3MP01" &&
                        ndisc.ExtractBasicInfo().Id.Substring(0, 6) != "R3IJ01")
                        throw new System.Exception();
                    ndisc.ExtractFiles(ext_f => true,
                    (f, ext_f) =>
                    {
                        var path = @".\tmp\nkit\DATA\" + (ext_f.PartitionId == null ? @"sys" : @"files");
                        if (ext_f.Path != "")
                            path += @"\" + ext_f.Path;
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        using (var stream = File.OpenWrite(path + @"\" + ext_f.Name))
                            f.Copy(stream, ext_f.Length);
                    });
                }
            }
            catch
            {
                result = false;
            }
            if (Directory.Exists(@".\Dats"))
                Directory.Delete(@".\Dats", true);
            if (Directory.Exists(@".\Processed"))
                Directory.Delete(@".\Processed", true);
            if (Directory.Exists(@".\Recovery"))
                Directory.Delete(@".\Recovery", true);
            if (!result)
                Directory.Delete(@".\tmp\nkit", true);
            return result;
        }
    }
}
