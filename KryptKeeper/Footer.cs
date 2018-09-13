using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KryptKeeper
{
    internal class Footer
    {
        public static readonly string FOOTER_TAG = "[KRYPTKEEPER]";
        private static readonly byte[] FOOTER_SIGNATURE = Utils.GetBytes(FOOTER_TAG);

        public DateTime AccessedTime { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Name { get; set; }

        public static Footer FromString(string footerString)
        {
            var footerSplit = footerString.Replace(FOOTER_TAG, "").Split(',');
            string name = Utils.GetRandomAlphanumericString(8) + ".krpt.place"; // Default if name's blank
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
                CreationTime = DateTime.FromFileTime(created),
                ModifiedTime = DateTime.FromFileTime(modified),
                AccessedTime = DateTime.FromFileTime(accessed)
            };
            return newFooter;
        }

        public void Build(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            Name = Path.GetFileName(path);
            CreationTime = File.GetCreationTime(path);
            ModifiedTime = File.GetLastWriteTime(path);
            AccessedTime = File.GetLastAccessTime(path);
        }

        public byte[] ToArray()
        {
            return Utils.GetBytes(ToString());
        }

        public override string ToString()
        {
            return FOOTER_TAG + "name:" + Name + ",creationTime:" + CreationTime.ToFileTime() +
                   ",modifiedTime:" + ModifiedTime.ToFileTime() + ",accessedTime:" + AccessedTime.ToFileTime();
        }

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
                for (int i = footerArray.Length - 1; i >= 0; i--)
                {
                    if (footerArray[i] != FOOTER_SIGNATURE[0]) continue;
                    if (!FOOTER_SIGNATURE.SequenceEqual(footerArray.Skip(i).Take(FOOTER_SIGNATURE.Length))) continue;
                    var newFooter = FromString(Encoding.UTF8.GetString(footerArray.Skip(i).ToArray()));
                    Name = newFooter.Name;
                    CreationTime = newFooter.CreationTime;
                    ModifiedTime = newFooter.ModifiedTime;
                    AccessedTime = newFooter.AccessedTime;
                    return true;
                }
            }
            return false;
        }
    }
}