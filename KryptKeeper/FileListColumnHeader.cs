using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KryptKeeper
{
    internal class FileListColumnHeader
    {
        private readonly string name;

        public FileListColumnHeader(string name)
        {
            this.name = name;
        }

        public string Name()
        {
            return name;
        }
    }
}
