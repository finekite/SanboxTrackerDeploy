using SandBoxEnviorments.Repositories;
using SandBoxEnviorments.Services;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace SandBoxEnviorments
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IUnityContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeMainWindow();
            Application.Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            container = new UnityContainer();
            container.RegisterType<IRepository, ExcelRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISandboxInfoService, SandboxInfoExcelService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDeployService, VSCommandPromptDeployService>(new ContainerControlledLifetimeManager());
        }

        private void ComposeMainWindow()
        {
            Application.Current.MainWindow = container.Resolve<MainWindow>();
        }
    }
}
