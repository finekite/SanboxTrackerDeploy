
using SandBoxEnviorments.Enums;
using System.Windows.Media;

namespace SandBoxEnviorments
{
    public class Sandbox
    {

        public string SandboxNumber { get; set; }

        public string DateLastDeployed { get; set; }

        public bool Deployable { get; set; }

        public string Status { get; set; }

        public string UserStory { get; set; }

        public string Developer { get; set; }

        public string BranchToDeploy { get; set; }

        public SolidColorBrush ColorOfSandbox { get; set; }

        public string LocalPathToSandBox { get; set; }

    }
}
