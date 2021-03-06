﻿using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KryptKeeper
{
    public class Footer
    {
        public static readonly string FOOTER_TAG = "[KRYPTKEEPER]";
        public static readonly byte[] FOOTER_SIGNATURE = Utils.GetBytes(FOOTER_TAG);
        
        //public Version Version { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime AccessedTime { get; set; }

        public static Footer FromString(string footerString)
        {
            var footerSplit = footerString.Replace(FOOTER_TAG, "").Split(',');
            string name = Utils.GetRandomAlphanumericString(8) + ".krpt.place"; // Default if name's blank
            long created = 0, modified = 0, accessed = 0;
            //var version = new Version();
            foreach (var data in footerSplit)
            {
                var item = data.Split(':')[0];
                var value = data.Split(':')[1];
                switch (item)
                {
                    //case "version":
                        //version = Version.Parse(value);
                        //break;
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
                //Version = version,
                Name = name,
                CreationTime = DateTime.FromFileTime(created),
                ModifiedTime = DateTime.FromFileTime(modified),
                AccessedTime = DateTime.FromFileTime(accessed)
            };
            return newFooter;
        }

        public void Build(string path, Version version)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            //Version = version;
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
        { //"version:" + Version +
            return FOOTER_TAG + "name:" + Name + ",creationTime:" + CreationTime.ToFileTime() +
                   ",modifiedTime:" + ModifiedTime.ToFileTime() + ",accessedTime:" + AccessedTime.ToFileTime();
        }

        public bool TryExtract(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            using (var rwStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                if (rwStream.Length > 1024)
                    rwStream.Seek(-1024, SeekOrigin.End);
                var footerArray = new byte[1024];
                rwStream.Read(footerArray, 0, 1024);
                for (int i = footerArray.Length - 1; i >= 0; i--)
                {
                    if (footerArray[i] != FOOTER_SIGNATURE[0]) continue;
                    if (!FOOTER_SIGNATURE.SequenceEqual(footerArray.Skip(i).Take(FOOTER_SIGNATURE.Length))) continue;
                    var newFooter = FromString(Encoding.UTF8.GetString(footerArray.Skip(i).ToArray()));
                    //Version = newFooter.Version;
                    Name = newFooter.Name;
                    CreationTime = newFooter.CreationTime;
                    ModifiedTime = newFooter.ModifiedTime;
                    AccessedTime = newFooter.AccessedTime;
                    rwStream.SetLength(rwStream.Length - newFooter.ToArray().Length);
                    return true;
                }
            }
            return false;
        }
    }
}