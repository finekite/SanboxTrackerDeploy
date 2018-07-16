using SandBoxEnviorments.Files;
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
            throw new NotImplementedException();
        }

        public void UpdateSandboxInfo(Sandbox sandbox)
        {
            var fileInfo = filemanager.GetFile();

            if (!fileInfo.Exists)
            {
                filemanager.CreateFile();
            }

            var sandboxList = ReadSandboxInfoFromFile(fileInfo);

            if (sandboxList != null)
            {
                var sandboxToUpdate = sandboxList.Where(x => x.SandboxNumber == sandbox.SandboxNumber).FirstOrDefault();

                if (sandboxToUpdate != null)
                {
                    sandboxToUpdate = sandbox;
                }
                else
                {
                    sandboxList.Add(sandbox);
                }
            }
            else
            {
                sandboxList = new ObservableCollection<Sandbox>();
                sandboxList.Add(sandbox);
            }

            var formatter = new BinaryFormatter();
            using (Stream stream = fileInfo.OpenWrite())
            {
                formatter.Serialize(stream, sandboxList);   
            }
        }
    }
}
