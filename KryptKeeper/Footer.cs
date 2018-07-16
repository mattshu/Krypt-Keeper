using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KryptKeeper
{
    internal class Footer
    {
        public const string FOOTER_TAG = "[KRYPTKEEPER]";
        private readonly byte[] FOOTER_SIGNATURE = Encoding.Default.GetBytes(FOOTER_TAG);

        public string Name { get; set; }
        //public string MD5 { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime AccessedTime { get; set; }

        public void Build(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            Name = Path.GetFileName(path);
            //MD5 = Helper.GetMD5StringFromPath(path);
            CreationTime = File.GetCreationTime(path);
            ModifiedTime = File.GetLastWriteTime(path);
            AccessedTime = File.GetLastAccessTime(path);
        }
        // TODO only works if file contains one footer
        public bool TryExtract(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);
            using (var rStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (rStream.Length > 1024)
                {
                    rStream.Seek(-1024, SeekOrigin.End);
                }
                var footerArray = new byte[1024];
                rStream.Read(footerArray, 0, 1024);
                if (rStream.Length < 1024)
                    Array.Resize(ref footerArray, (int)rStream.Length);
                for (int i = 0; i < footerArray.Length; i++)
                {
                    if (footerArray[i] != FOOTER_SIGNATURE[0]) continue;
                    if (!FOOTER_SIGNATURE.SequenceEqual(footerArray.Skip(i).Take(FOOTER_SIGNATURE.Length))) continue;
                    var newFooter = FromString(Encoding.Default.GetString(footerArray.Skip(i).ToArray()));
                    Name = newFooter.Name;
                    //MD5 = newFooter.MD5;//
                    CreationTime = newFooter.CreationTime;
                    ModifiedTime = newFooter.ModifiedTime;
                    AccessedTime = newFooter.AccessedTime;
                    return true;
                }
            }
            return false;
        }

        /*public bool TryExtract(byte[] data)
        {
            var decoded = "";
            for (int i = data.Length - FOOTER_SIGNATURE.Length; i >= 0; i--)
            {
                if (data[i] != FOOTER_SIGNATURE[0]) continue;
                var read = new byte[FOOTER_SIGNATURE.Length];
                Array.Copy(data, i, read, 0, read.Length);
                if (!read.SequenceEqual(FOOTER_SIGNATURE)) continue;
                var footerBytes = new byte[data.Length - i];
                Array.Copy(data, i, footerBytes, 0, footerBytes.Length);
                decoded = Encoding.Default.GetString(footerBytes);
                break;
            }
            if (string.IsNullOrEmpty(decoded))
                return false; // Cannot find footer
            var newFooter = FromString(decoded);
            Name = newFooter.Name;
            MD5 = newFooter.MD5;
            CreationTime = newFooter.CreationTime;
            ModifiedTime = newFooter.ModifiedTime;
            AccessedTime = newFooter.AccessedTime;
            return true;
        }*/

        public byte[] ToArray()
        {
            return Encoding.Default.GetBytes(ToString());
        }

        public static Footer FromString(string footerString)
        {
            var footerSplit = footerString.Replace(FOOTER_TAG, "").Split(',');
            string name = "", md5 = "";
            long created = 0, modified = 0, accessed = 0;
            foreach (var data in footerSplit)
            {
                var item = data.Split(':')[0];
                var value = data.Split(':')[1];
                switch (item)
                {
                    case "name":
                        name = value;
                        break;
                    case "md5":
                        md5 = value;
                        break;
                    case "creationTime":
                        created = long.Parse(value);
                        break;
                    case "modifiedTime":
                        modified = long.Parse(value);
                        break;
                    case "accessedTime":
                        accessed = long.Parse(value);
                        break;
                }
            }
            var newFooter = new Footer
            {
                Name = name,
                //MD5 = md5,
                CreationTime = DateTime.FromFileTime(created),
                ModifiedTime = DateTime.FromFileTime(modified),
                AccessedTime = DateTime.FromFileTime(accessed)
            };
            return newFooter;
        }

        public override string ToString()
        {//",md5:" + MD5 + 
            return FOOTER_TAG + "name:" + Name + ",creationTime:" + CreationTime.ToFileTime() +
                   ",modifiedTime:" + ModifiedTime.ToFileTime() + ",accessedTime:" + AccessedTime.ToFileTime();
        }
    }
}