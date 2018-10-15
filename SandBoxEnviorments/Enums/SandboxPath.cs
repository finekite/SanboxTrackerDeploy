
using System.Collections.Generic;
using System.Linq;

namespace SandBoxEnviorments.Enums
{
    public class SandboxPath : Enumeration
    {
        public readonly string SandboxfilePath;

        public SandboxPath(int id, string displayValue, string sandboxFilePath) : base(id, displayValue)
        {
            SandboxfilePath = sandboxFilePath;
        }

        public static readonly SandboxPath SANDBOX1 = new SandboxPath(1, "SandBox One", @"Cd\Code\<you-path-here>");

        public static readonly SandboxPath SANDBOX2 = new SandboxPath(2, "SandBox Two", @"Cd\Code\<you-path-here>");

        public static readonly SandboxPath SANDBOX3 = new SandboxPath(3, "SandBox Three", @"Cd\Code\<you-path-here>");

        public static readonly SandboxPath SANDBOX4 = new SandboxPath(4, "SandBox Four", @"Cd\Code\<you-path-here>");

        public static readonly SandboxPath SANDBOX5 = new SandboxPath(5, "SandBox Five", @"Cd\Code\<you-path-here>");

        private static IEnumerable<SandboxPath> AllSandBoxes
        {
            get
            {
                return new List<SandboxPath>()
                {
                    SANDBOX1, SANDBOX2, SANDBOX3, SANDBOX4, SANDBOX5
                };
            }
        }

        public static SandboxPath TryGetSandbox(int sandBoxNumber)
        {
            return AllSandBoxes.Where(x => x.id == sandBoxNumber).FirstOrDefault();
        }
    }
}
