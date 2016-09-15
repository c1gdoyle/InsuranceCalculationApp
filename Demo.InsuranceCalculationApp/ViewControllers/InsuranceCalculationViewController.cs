using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Presentation.Base;
using Prism.Commands;

namespace Demo.InsuranceCalculationApp.ViewControllers
{
    public class InsuranceCalculationViewController : ViewControllerBase
    {
        public InsuranceCalculationViewController()
        {
            InitialiseCommands();
        }

        private void InitialiseCommands()
        {
            ExitCommand = new DelegateCommand(() => NotifiyWindowShouldClose(null));
        }

        #region Properties
        public string Title
        {
            get { return "Insurance Premium Calculator"; }
        }
        #endregion Properties

        #region Commands
        public DelegateCommand ExitCommand
        {
            get;
            private set;
        }
        #endregion Commands
    }
}
