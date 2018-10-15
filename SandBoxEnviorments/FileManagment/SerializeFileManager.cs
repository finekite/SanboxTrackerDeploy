using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.FileManagement
{
    public class SerializeFileManager : IFileManager
    {
        private string file;

        private string directory;

        public SerializeFileManager(string file, string directory)
        {
            this.file = file;
            this.directory = directory;
        }

        public void CreateDirectory()
        {
            Directory.CreateDirectory(directory);
        }

        public void CreateFile()
        {
            if (!Directory.Exists(directory))
            {
                CreateDirectory();
            }

            var fileInfo = new FileInfo(file);

            fileInfo.Create();
        }

        public FileInfo GetFile()
        {
            var fileInfo = new FileInfo(file);

            if (!fileInfo.Exists)
            {
                CreateFile();
            }

            return fileInfo;
        }
    }
}
