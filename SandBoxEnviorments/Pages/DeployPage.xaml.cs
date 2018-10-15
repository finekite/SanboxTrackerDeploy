using SandBoxEnviorments.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SandBoxEnviorments
{
    /// <summary>
    /// Interaction logic for DeployPage.xaml
    /// </summary>
    public partial class DeployPage : Page
    {
        private ISandboxInfoService sandboxInfoService;

        private IDeployService deployService;

        public Sandbox SandBoxInfo { get; set; }

        public DeployPage(ISandboxInfoService service, IDeployService deployService, Sandbox sandBox)
        {
            InitializeComponent();
            this.sandboxInfoService = service;
            this.deployService = deployService;

            ValidateSandBoxInfo(sandBox);

            SandBoxInfo = sandBox;
            this.DataContext = SandBoxInfo;
        }

        private void ValidateSandBoxInfo(Sandbox sandBox)
        {
            if (string.IsNullOrEmpty(sandBox.Developer))
            {
                sandBox.Developer = Environment.UserName;
            }
        }

        private void Deploy_Button_Click(object sender, RoutedEventArgs e)
        {
            SandBoxInfo.BranchToDeploy = branchToDeploy.Text;

            try
            {
                var deployed = deployService.DeploySandBox(SandBoxInfo);

                if (deployed)
                {
                    sandboxInfoService.UpdateSandboxInfo(SandBoxInfo);
                    NavigationService.Navigate(new HomePage(sandboxInfoService, deployService));
                }
                else
                {
                    MessageBox.Show("Sandbox was not deployed please check the loggy woggies");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sandbox was not deployed. {ex.Message}");
            }
        }
    }
}
