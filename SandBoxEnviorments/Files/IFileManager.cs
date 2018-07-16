using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.Files
{
    public interface IFileManager
    {
        FileInfo GetFile();

        void CreateFile();

        void CreateDirectory();
    }
}
