using System.Windows;
using Microsoft.Practices.Unity;

namespace SimpleQRCodeSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            //container.RegisterType<ICustomerService, CustomerService>();

            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
