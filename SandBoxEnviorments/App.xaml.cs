﻿using SandBoxEnviorments.FileManagement;
using SandBoxEnviorments.Repositories;
using SandBoxEnviorments.Services;
using System;
using System.Configuration;
using System.Windows;
using Unity;
using Unity.Injection;
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
            container.RegisterType<IFileManager, SerializeFileManager>(new InjectionConstructor(GetConfiguration("SerializeFileManagerFileName"), GetConfiguration("SerializeFileManagerDirectoryName")));
            container.RegisterType<IRepository, SerializeRepositoy>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISandboxInfoService, SandboxInfoExcelService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDeployService, VSCommandPromptDeployService>(new ContainerControlledLifetimeManager());
        }

        private void ComposeMainWindow()
        {
            Application.Current.MainWindow = container.Resolve<MainWindow>();
        }

        private string GetConfiguration(string fileManagerType)
        {
            return ConfigurationManager.AppSettings[fileManagerType];
        }
    }
}
