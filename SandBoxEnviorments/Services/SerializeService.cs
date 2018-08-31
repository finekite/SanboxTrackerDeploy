using SandBoxEnviorments.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.Services
{
    public class SerializeService : ISandboxInfoService
    {
        private IRepository serializeRepository;

        public SerializeService(IRepository serializeRepository)
        {
            this.serializeRepository = serializeRepository;
        }

        public void AddNewSandboxFile(FileInfo fileInfo)
        {
            serializeRepository.AddNewSandboxFile();
        }

        public ObservableCollection<Sandbox> GetSandboxesInfo()
        {
           return serializeRepository.GetSandboxesInfo();
        }

        public bool SignOffOnSanbox(Sandbox sandbox)
        {
            return serializeRepository.SignOffOnSanbox(sandbox);
        }

        public void UpdateSandboxInfo(Sandbox sandbox)
        {
            var sandboxList = serializeRepository.GetSandboxesInfo();
            UpdateSandboxList(sandboxList, sandbox);
            serializeRepository.UpdateSandboxInfo(sandbox);
        }

        private void UpdateSandboxList(ObservableCollection<Sandbox> sandboxList, Sandbox sandbox)
        {
            if (sandboxList != null)
            {
                int? sandboxElement = sandboxList.Select((sandboxToUpdate, index) => new { sandboxToUpdate, index })
                          .Where(x => x.sandboxToUpdate.SandboxNumber == sandbox.SandboxNumber)
                          .Select(sandboxIndex => (int?)sandboxIndex.index)
                          .FirstOrDefault();

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
            else
            {
                sandboxList = new ObservableCollection<Sandbox>();
                sandboxList.Add(sandbox);
            }
        }
    }
}
