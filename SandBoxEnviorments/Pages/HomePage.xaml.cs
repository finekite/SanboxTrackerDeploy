using SandBoxEnviorments.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SandBoxEnviorments
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public ObservableCollection<Sandbox> Boxes { get; set; } 

        private ISandboxInfoService sandboxInfoService; 

        private IDeployService deployService;

        public HomePage(ISandboxInfoService service, IDeployService deployService)
        {
            InitializeComponent();

            this.sandboxInfoService = service;
            this.deployService = deployService;

            GetSandboxes();
        }

        private void GetSandboxes()
        {
            Boxes = sandboxInfoService.GetSandboxesInfo();

            if (Boxes == null || !Boxes.Any())
            {
                SandBox.Visibility = Visibility.Hidden;
                noSandboxesMessage.Visibility = Visibility.Visible;
            }
            else
            {
                SandBox.ItemsSource = Boxes;
            }
        }


        private void Deploy_Sanbox_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Sandbox)SandBox.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show(Application.Current.MainWindow, "Please select Sandbox to deploy");
            }
            else if(!selectedItem.Deployable)
            {
                MessageBox.Show(Application.Current.MainWindow, $"Sandbox is not deployable at this time. Please contact {selectedItem.Developer} to sign off");
            }
            else
            {
                this.NavigationService.Navigate(new DeployPage(sandboxInfoService, deployService, selectedItem));
            }
        }


        private void Sign_Off_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Sandbox)SandBox.SelectedItem;
            if (selectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, "Please select Sandbox to sign off");
            }
            else if (string.IsNullOrEmpty(selectedItem.Developer))
            {
                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, $@"Sanbox is not deployed. No need to sign off");
            }
            else if (selectedItem.Developer != Environment.UserName)
            {
                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, $@"You are not the developer on this project and cannot sign off. Please see {selectedItem.Developer} to sign off");
            }
            else
            {
                sandboxInfoService.SignOffOnSanbox(selectedItem);
                NavigationService.Navigate(new HomePage(sandboxInfoService, deployService));
            }
        }

        private void Add_Sandbox_Button_Click(object sender, RoutedEventArgs e)
        {
            var sandbox = new Sandbox();
            int lastSandBoxNumber = Boxes != null && Boxes.Any() ? int.Parse(Boxes.Last().SandboxNumber) + 1 : 1;
            sandbox.SandboxNumber = lastSandBoxNumber.ToString();
            NavigationService.Navigate(new DeployPage(sandboxInfoService, deployService, sandbox));
        }


        private void SandBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
