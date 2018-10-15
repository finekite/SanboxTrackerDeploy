using SandBoxEnviorments.FileManagement;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SandBoxEnviorments.Repositories
{
    public class SerializeRepositoy : IRepository
    {
        private IFileManager filemanager;

        public SerializeRepositoy(IFileManager filemanager)
        {
            this.filemanager = filemanager;
        }

        public void AddNewSandboxFile()
        {
            filemanager.CreateFile();
        }

        public ObservableCollection<Sandbox> GetSandboxesInfo()
        {
            var fileinfo = filemanager.GetFile();

            var sandboxes = ReadSandboxInfoFromFile(fileinfo);

            return sandboxes;
        }

        private ObservableCollection<Sandbox> ReadSandboxInfoFromFile(FileInfo fileinfo)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if(fileinfo.Length > 0)
            {
                using (Stream stream = fileinfo.OpenRead())
                {
                    var sandobxes = (ObservableCollection<Sandbox>)formatter.Deserialize(stream);
                    sandobxes = SetSandBoxColor(sandobxes);
                    return sandobxes;
                }
            }
            else
            {
                return null;
            }
        }

        private ObservableCollection<Sandbox> SetSandBoxColor(ObservableCollection<Sandbox> sandobxes)
        {
            foreach (var sandbox in sandobxes)
            {
                sandbox.ColorOfSandbox = sandbox.Deployable ? "Green" : "Red";
            }

            return sandobxes;
        }

        public bool SignOffOnSanbox(Sandbox sandbox)
        {
            sandbox.Developer = null;
            sandbox.Deployable = true;
            UpdateSandboxInfo(sandbox);

            return true;
        }

        public void UpdateSandboxInfo(Sandbox sandbox)
        {
            var fileInfo = filemanager.GetFile();

            if (!fileInfo.Exists)
            {
                filemanager.CreateFile();
            }

            var sandboxList = ReadSandboxInfoFromFile(fileInfo);

            UpdateSandboxList(sandboxList, sandbox);

            SerializeSandboxList(sandboxList, fileInfo);
        }

        private void SerializeSandboxList(ObservableCollection<Sandbox> sandboxList, FileInfo fileInfo)
        {
            var formatter = new BinaryFormatter();
            using (Stream stream = fileInfo.OpenWrite())
            {
                formatter.Serialize(stream, sandboxList);
            }
        }

        private void UpdateSandboxList(ObservableCollection<Sandbox> sandboxList, Sandbox sandbox)
        {
            if (sandboxList != null)
            {
                // get the sandbox ur trying to update
                int? sandboxElement = sandboxList.Select((sandboxToUpdate, index) => new { sandboxToUpdate, index })
                          .Where(x => x.sandboxToUpdate.SandboxNumber == sandbox.SandboxNumber)
                          .Select(sandboxIndex => (int?)sandboxIndex.index)
                          .FirstOrDefault();

                // if sandbox exist update it if not add it to the list
                if (sandboxElement.HasValue)
                {
                    sandboxList.RemoveAt(sandboxElement.Value);
                    sandboxList.Insert(sandboxElement.Value, sandbox);
                }
                else
                {
                    sandboxList.Add(sandbox);
                }
            }
            // if sandboxList is null it means there are no sandboxes yet. Add the first one here
            else
            {
                sandboxList = new ObservableCollection<Sandbox>();
                sandboxList.Add(sandbox);
            }
        }
    }
}
