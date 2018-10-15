using System.Windows;
using SandBoxEnviorments.Services;

namespace SandBoxEnviorments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ISandboxInfoService sandboxInfoService;

        private IDeployService deployService;

        public MainWindow(ISandboxInfoService sandboxInfoService, IDeployService deployService)
        {
            InitializeComponent();

            this.sandboxInfoService = sandboxInfoService;
            this.deployService = deployService;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.NavigationService.Navigate(new HomePage(sandboxInfoService, deployService));
        }
    }
}
