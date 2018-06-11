

using OfficeOpenXml;
using SandBoxEnviorments.Enums;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace SandBoxEnviorments.Repositories
{
    public class ExcelRepository : IRepository
    {
        private ObservableCollection<Sandbox> sandboxes;

        public ObservableCollection<Sandbox> GetSandboxesInfo()
        {
            var fileInfo = GetFile();

            if (!fileInfo.Exists)
            {
                AddNewSandboxFile(fileInfo);
            }

            ReadSandboxInfo(fileInfo);

            return sandboxes;
        }

        public void UpdateSandboxInfo(Sandbox sandbox)
        {
            sandbox.Deployable = false;
            var fileInfo = GetFile();
            WriteSandboxInfo(sandbox, fileInfo);
        }


        public void AddNewSandboxFile(FileInfo fileInfo)
        {
            byte firstRow = 1;
            using (var excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                foreach (var item in SandboxColumnsEnums.AllSandboxColumns)
                {
                    worksheet.Cells[firstRow, item.id].Value = item.ObjectName;
                }

                excelPackage.Save();
            }
        }

        public bool SignOffOnSanbox(Sandbox sandbox)
        {
            var fileInfo = GetFile();
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets["Sheet1"];
                int rowNum = excelWorksheet.Row(int.Parse(sandbox.SandboxNumber)).Row + 1;
                int deployableColumn = GetColumn(excelWorksheet, "Deployable");
                int developercolumn = GetColumn(excelWorksheet, "Developer");
                excelWorksheet.Cells[rowNum, deployableColumn].Value = true;
                excelWorksheet.Cells[rowNum, developercolumn].Value = null;
                excelPackage.Save();
            }
            return true;
        }

        private void WriteSandboxInfo(Sandbox sandbox, FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets["Sheet1"];
                    int rowNum = excelWorksheet.Row(int.Parse(sandbox.SandboxNumber)).Row + 1;
                    excelWorksheet.Cells[rowNum, 1].Value = sandbox.SandboxNumber;
                    excelWorksheet.Cells[rowNum, 2].Value = sandbox.Developer;
                    excelWorksheet.Cells[rowNum, 3].Value = sandbox.DateLastDeployed;
                    excelWorksheet.Cells[rowNum, 4].Value = sandbox.Status;
                    excelWorksheet.Cells[rowNum, 5].Value = sandbox.UserStory;
                    excelWorksheet.Cells[rowNum, 6].Value = sandbox.Deployable;

                    excelPackage.Save();
                }
            }
        }

        private FileInfo GetFile()
        {
            var fileName = "SandboxInfo.xlsx";
            string projectDirectory = @"O:\TKO\"; //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName; /* */
            return new FileInfo(projectDirectory + fileName);
        }

        private void ReadSandboxInfo(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets["Sheet1"];
                    sandboxes = new ObservableCollection<Sandbox>();
                    var sandBox = new Sandbox();
                    for (int row = excelWorksheet.Dimension.Start.Row + 1;
                            row <= excelWorksheet.Dimension.End.Row;
                         row++)
                    {
                        for (int column = excelWorksheet.Dimension.Start.Column;
                                 column <= excelWorksheet.Dimension.End.Column;
                                 column++)
                        {
                            string cellValue = excelWorksheet.Cells[row, column].Text;
                            MapCellValueToSandBox(cellValue, sandBox, column);
                        }

                        sandBox.ColorOfSandbox = DetermineSandBoxColor(sandBox.Deployable);
                        sandBox.LocalPathToSandBox = SandboxPath.TryGetSandbox(int.Parse(sandBox.SandboxNumber))?.SandboxfilePath ?? null;
                        sandboxes.Add(sandBox);
                        sandBox = new Sandbox();
                    }
                }
            }
        }

        private SolidColorBrush DetermineSandBoxColor(bool deployable)
        {
            if (deployable)
            {
                return new SolidColorBrush(Colors.Green);
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        private void MapCellValueToSandBox(string cellValue, Sandbox sandbox, int column)
        {
            var columnName = GetSanboxColumnName(column);
            var sandboxPropertyMatchingColumnName = sandbox.GetType().GetProperty(columnName.ObjectName);

            if (sandboxPropertyMatchingColumnName.Name == SandboxColumnsEnums.DEPLOYABLE.ObjectName)
            {
                bool deployable = cellValue == "1" ? true : false;
                sandboxPropertyMatchingColumnName.SetValue(sandbox, deployable);
            }
            else
            {
                sandboxPropertyMatchingColumnName.SetValue(sandbox, cellValue);
            }
        }

        private SandboxColumnsEnums GetSanboxColumnName(int column)
        {
            return SandboxColumnsEnums.AllSandboxColumns.Where(x => x.id == column).FirstOrDefault();
        }

        private int GetColumn(ExcelWorksheet excelWorksheet, string columnName)
        {
            return excelWorksheet.Cells["1:1"].FirstOrDefault(x => x.Value.ToString() == columnName).Start.Column;
        }
    }
}
