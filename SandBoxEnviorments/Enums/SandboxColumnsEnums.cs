using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.Enums
{
    public class SandboxColumnsEnums : Enumeration
    {
        public string ObjectName;

        public SandboxColumnsEnums(int id, string displayValue, string objectName) : base(id, displayValue)
        {
            ObjectName = objectName;
        }

        public static readonly SandboxColumnsEnums SANDBOX = new SandboxColumnsEnums(1, "The sandbox number", "SandboxNumber");

        public static readonly SandboxColumnsEnums DEVELOPER = new SandboxColumnsEnums(2, "The developer using the sandbox", "Developer");

        public static readonly SandboxColumnsEnums DATELASTDEPLOYED = new SandboxColumnsEnums(3, "The date sandbox was last deployed", "DateLastDeployed");

        public static readonly SandboxColumnsEnums STATUS = new SandboxColumnsEnums(4, "The deploy status of the sandbox", "Status");

        public static readonly SandboxColumnsEnums USERSTORY = new SandboxColumnsEnums(5, "The user story", "UserStory");

        public static readonly SandboxColumnsEnums DEPLOYABLE = new SandboxColumnsEnums(6, "The flas indication if sandbox is deployable", "Deployable");

        public static IEnumerable<SandboxColumnsEnums> AllSandboxColumns
        {
            get
            {
                return new List<SandboxColumnsEnums>()
                {
                    SANDBOX, DEVELOPER, DATELASTDEPLOYED, STATUS, USERSTORY, DEPLOYABLE
                };
            }
        }

    }
}
