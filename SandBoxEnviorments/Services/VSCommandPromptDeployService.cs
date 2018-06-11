
using SandBoxEnviorments.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SandBoxEnviorments.Services
{

    public class VSCommandPromptDeployService : IDeployService
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private readonly string microsoftVSToolsPath = @"Cd\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\Tools";

        private readonly string vsCommandPromt= @"VsDevCmd.bat";

        private readonly string publishCommand = "publish_lending.bat";

        public bool DeploySandBox(Sandbox sandbox)
        {
            try
            {
                var process = IniateCommandPromtProcess();
                ExecuteCommandPromptProcess(process, sandbox);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void ExecuteCommandPromptProcess(Process process, Sandbox sandbox)
        {
            // start the computer process
            process.Start();

            // run VsDevCmd.bat to set up command prompt for visual studio developer tools
            process.StandardInput.WriteLine(microsoftVSToolsPath);
            process.StandardInput.WriteLine(vsCommandPromt);

            // navigate to sandbox path
            process.StandardInput.WriteLine(sandbox.LocalPathToSandBox);

            // execute publish scripts
            process.StandardInput.WriteLine($@"{publishCommand} ""{sandbox.BranchToDeploy}""");

        }


        private Process IniateCommandPromtProcess()
        {
            var processInfo = new ProcessStartInfo("cmd.exe")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
            };

            var process = new Process()
            {
                StartInfo = processInfo,
            };

            return process;
        }
    }
}
