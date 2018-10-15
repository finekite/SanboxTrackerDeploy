using OfficeOpenXml;
using SandBoxEnviorments.Enums;
using SandBoxEnviorments.FileManagement;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SandBoxEnviorments.Repositories
{
    public class ExcelRepository : IRepository
    {
        private const string sheetName = "Sheet1";

        private IFileManager filemanager;

        public ExcelRepository(IFileManager filemanager)
        {
            this.filemanager = filemanager;
        }


        public ObservableCollection<Sandbox> GetSandboxesInfo()
        {
            var fileInfo = filemanager.GetFile();

            if (!fileInfo.Exists)
            {
                AddNewSandboxFile();
            }

            var sandboxes = ReadSandboxInfo(fileInfo);

            return sandboxes;
        }

        public void UpdateSandboxInfo(Sandbox sandbox)
        {
            sandbox.Deployable = false;

            var fileInfo = filemanager.GetFile();

            Update(sandbox, fileInfo);
        }

        public void AddNewSandboxFile()
        {
            filemanager.CreateFile();
        }

        public bool SignOffOnSanbox(Sandbox sandbox)
        {
            sandbox = ClearUser(sandbox);

            var fileInfo = filemanager.GetFile();

            Update(sandbox, fileInfo);

            return true;
        }

        private Sandbox ClearUser(Sandbox sandbox)
        {
            sandbox.Deployable = true;
            sandbox.Developer = null;
            return sandbox;
        }

        private void Save(ExcelPackage package)
        {
            try
            {
                package.Save();
            }
            catch (InvalidOperationException ex)
            {
                //TODO: log error here
            }
        }

        private void Update(Sandbox sandbox, FileInfo fileInfo)
        {
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorkbook book = package.Workbook;
                ExcelWorksheet sheet = book.Worksheets[sheetName];

                int rowNum = GetRowNumberFromSandBox(sandbox, sheet);

                sheet.Cells[rowNum, 1].Value = sandbox.SandboxNumber;
                sheet.Cells[rowNum, 2].Value = sandbox.Developer;
                sheet.Cells[rowNum, 3].Value = sandbox.DateLastDeployed;
                sheet.Cells[rowNum, 4].Value = sandbox.Status;
                sheet.Cells[rowNum, 5].Value = sandbox.UserStory;
                sheet.Cells[rowNum, 6].Value = sandbox.Deployable;

                Save(package);
            }
        }

        private ObservableCollection<Sandbox> ReadSandboxInfo(FileInfo fileInfo)
        {
            ObservableCollection<Sandbox> sandboxes = new ObservableCollection<Sandbox>();

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorkbook book = excelPackage.Workbook;
                ExcelWorksheet sheet = book.Worksheets[sheetName];

                for (int row = sheet.Dimension.Start.Row + 1; row <= sheet.Dimension.End.Row; row++)
                {
                    var tempSandbox = new Sandbox();

                    for (int column = sheet.Dimension.Start.Column; column <= sheet.Dimension.End.Column; column++)
                    {
                        string cellValue = sheet.Cells[row, column].Text;

                        MapCellValueToSandBox(cellValue, tempSandbox, column);
                    }

                    tempSandbox.ColorOfSandbox = DetermineSandBoxColor(tempSandbox.Deployable);

                    tempSandbox.LocalPathToSandBox = SandboxPath.TryGetSandbox(int.Parse(tempSandbox.SandboxNumber))?.SandboxfilePath ?? null;

                    sandboxes.Add(tempSandbox);
                }
            }
            return sandboxes;
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


        private int GetRowNumberFromSandBox(Sandbox sandbox, ExcelWorksheet sheet) => sheet.Row(int.Parse(sandbox.SandboxNumber)).Row + 1;

        private SandboxColumnsEnums GetSanboxColumnName(int column) => SandboxColumnsEnums.AllSandboxColumns.Where(x => x.id == column).FirstOrDefault();

        private int GetColumn(ExcelWorksheet sheet, string columnName) => sheet.Cells["1:1"].FirstOrDefault(x => x.Value.ToString() == columnName).Start.Column;

        private string DetermineSandBoxColor(bool deployable) => deployable ? "Green" : "Red";
    }
}
