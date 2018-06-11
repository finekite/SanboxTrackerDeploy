using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.Services
{
    public interface IDeployService
    {
        bool DeploySandBox(Sandbox sandbox);
    }
}
