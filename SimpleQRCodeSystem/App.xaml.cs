using System.Windows;
using Microsoft.Practices.Unity;
using SimpleQRCodeSystem.Repositories;
using SimpleQRCodeSystem.Repositories.Sqlite;
using SimpleQRCodeSystem.Services;

namespace SimpleQRCodeSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using (var unity = CreateContainerAndRegisterServices())
            {
                var mainWindow = unity.Resolve<MainWindow>();
                mainWindow.Show();
            }
        }

        private IUnityContainer CreateContainerAndRegisterServices()
        {
            UnityContainer container = null;

            try
            {
                container = new UnityContainer();
                // Register Repositories
                container.RegisterInstance<IBadgeRepository>(new BadgeRepository());

                //Register services.
                container.RegisterType<IBadgeService, BadgeService>();
            }
            catch
            {
                container?.Dispose();
                throw;
            }

            return container;
        }
    }
}
