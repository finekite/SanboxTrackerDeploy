using SandBoxEnviorments.Repositories;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SandBoxEnviorments.Services
{
    public class SandboxInfoExcelService : ISandboxInfoService
    {
        private Object thisLock = new Object();

        private IRepository repository;

        public SandboxInfoExcelService(IRepository repository)
        {
            this.repository = repository;
        }

        public ObservableCollection<Sandbox> GetSandboxesInfo()
        {
            lock(thisLock)
            {
                return repository.GetSandboxesInfo();
            }
        }

        public void UpdateSandboxInfo(Sandbox sandbox)
        {
            lock (thisLock)
            {
                repository.UpdateSandboxInfo(sandbox);
            }
        }

        public void AddNewSandboxFile(FileInfo fileInfo)
        {
            lock (thisLock)
            {
                repository.AddNewSandboxFile();
            }
        }

        public bool SignOffOnSanbox(Sandbox sandbox)
        {
            return repository.SignOffOnSanbox(sandbox);
        }
    }
}
