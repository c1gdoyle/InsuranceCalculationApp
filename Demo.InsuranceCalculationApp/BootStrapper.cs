using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Unity;

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
            return Container.TryResolve<InsuranceCalculationAppShell>();
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
            ModuleCatalog.AddModule(null);
        }
        #endregion UnityBootstrapper Overrides

    }
}
