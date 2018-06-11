
using System.Collections.ObjectModel;
using System.IO;

namespace SandBoxEnviorments.Repositories
{
    public interface IRepository
    {
        ObservableCollection<Sandbox> GetSandboxesInfo();

        void UpdateSandboxInfo(Sandbox sandbox);

        bool SignOffOnSanbox(Sandbox sandbox);

        void AddNewSandboxFile(FileInfo fileInfo);
    }
}
