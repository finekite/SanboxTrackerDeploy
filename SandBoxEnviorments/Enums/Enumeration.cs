using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxEnviorments.Enums
{
    public class Enumeration
    {
        public int id;

        public string displayValue;

        protected Enumeration(int id, string displayValue)
        {
            this.id = id;
            this.displayValue = displayValue;
        }
    }
}
