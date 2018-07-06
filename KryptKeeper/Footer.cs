using System;
using System.IO;
using System.Text;

namespace KryptKeeper
{
    internal class Footer
    {
        public const string FOOTER_TAG = "[KRYPTKEEPER]";

        public string Name { get; set; }
        public string MD5 { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime AccessedTime { get; set; }

        public void Build(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            Name = Path.GetFileName(path);
            MD5 = Helper.GetMD5StringFromPath(path);
            CreationTime = File.GetCreationTime(path);
            ModifiedTime = File.GetLastWriteTime(path);
            AccessedTime = File.GetLastAccessTime(path);
        }

        public void Extract(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            // TODO
        }

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
                MD5 = md5,
                CreationTime = DateTime.FromFileTime(created),
                ModifiedTime = DateTime.FromFileTime(modified),
                AccessedTime = DateTime.FromFileTime(accessed)
            };
            return newFooter;
        }

        public override string ToString()
        {
            return FOOTER_TAG + "name:" + Name + ",md5:" + MD5 + ",creationTime:" + CreationTime.ToFileTime() + ",modifiedTime:" + ModifiedTime.ToFileTime() + ",accessedTime:" + AccessedTime.ToFileTime();
        }


    }
}