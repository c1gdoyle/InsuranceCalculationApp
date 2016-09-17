using System.Windows;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Demo.InsuranceCalculation;
using Prism.Modularity;

namespace Demo.InsuranceCalculationApp
{
    /// <summary>
    /// Basic implemenation of <see cref="UnityBootstrapper"/>.
    /// </summary>
    public class BootStrapper : UnityBootstrapper
    {
        #region UnityBootstrapper Overrides
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<InsuranceCalculationAppShell>();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            App.Current.MainWindow = (InsuranceCalculationAppShell)Shell;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(InsuranceCalculationModule));
        }
        #endregion UnityBootstrapper Overrides
    }
}
