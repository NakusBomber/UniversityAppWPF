using Ninject;
using System.Configuration;
using System.Data;
using System.Windows;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.View.Pages;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Stores;
using UniversityApp.ViewModel.ViewModels;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _kernel = new StandardKernel();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
            _kernel.Bind<NavigationViewModel>().ToSelf();
            _kernel.Bind<INavigationStore>().To<NavigationStore>().InSingletonScope();

            _kernel.Bind<ShowViewModel>().ToSelf();
            _kernel.Bind<CourseViewModel>().ToSelf();
            _kernel.Bind<GroupViewModel>().ToSelf();
            _kernel.Bind<StudentViewModel>().ToSelf();
            _kernel.Bind<TeacherViewModel>().ToSelf();

            MainWindow = new MainWindow();
            MainWindow.DataContext = _kernel.Get<NavigationViewModel>();

            MainWindow.Show();
        }
    }

}
