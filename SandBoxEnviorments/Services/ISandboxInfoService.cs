
using System.Collections.ObjectModel;
using System.IO;

namespace SandBoxEnviorments.Services
{
    public interface ISandboxInfoService
    {
        ObservableCollection<Sandbox> GetSandboxesInfo();

        void UpdateSandboxInfo(Sandbox sandbox);

        bool SignOffOnSanbox(Sandbox sandbox);

        void AddNewSandboxFile(FileInfo fileInfo);
    }
}
