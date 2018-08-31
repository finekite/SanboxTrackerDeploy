using OfficeOpenXml;
using SandBoxEnviorments.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.FileManagement
{
    public class ExcelFileManager : IFileManager
    {
        private string file;

        private string directory;

        private const string sheetName = "Sheet1";

        public ExcelFileManager(string file, string directory)
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
            byte firstRow = 1;

            using (var package = new ExcelPackage(new FileInfo(file)))
            {
                ExcelWorkbook book = package.Workbook;

                ExcelWorksheet worksheet = book.Worksheets.Add(sheetName);

                foreach (var columnEnum in SandboxColumnsEnums.AllSandboxColumns)
                {
                    worksheet.Cells[firstRow, columnEnum.id].Value = columnEnum.ObjectName;
                }

                package.Save();
            }
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
