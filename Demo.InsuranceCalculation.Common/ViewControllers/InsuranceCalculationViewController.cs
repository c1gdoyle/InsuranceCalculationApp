using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Services;
using Demo.Presentation.Base;
using Prism.Commands;

namespace Demo.InsuranceCalculation.ViewControllers
{
    public class InsuranceCalculationViewController : ViewControllerBase
    {
        #region Private Members
        private readonly IInsurancePolicyAssessmentService _policyAssessmentService;

        private string _driverName;
        private Occupation? _selectedOccupation;
        private DateTime? _dateOfBirth;

        private readonly ObservableCollection<Driver> _drivers;
        private Driver _selectedDriver;

        #endregion Private Members


        public InsuranceCalculationViewController(IInsurancePolicyAssessmentService policyAssessmentService)
        {
            _policyAssessmentService = policyAssessmentService;
            _drivers = new ObservableCollection<Driver>();

            InitialiseCommands();
        }

        private void InitialiseCommands()
        {
            AddDriverCommand = new DelegateCommand(() => OnAddDriver(), () => CanAddDriver);
            ClearDriverDetailsCommand = new DelegateCommand(() => OnClearDriverDetails());
        }

        #region Properties
        public string DriverName
        {
            get { return _driverName; }
            set
            {
                _driverName = value;
                NotifyPropertyChanged("DriverName");
                AddDriverCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Occupation> Occupations
        {
            get { return (IEnumerable<Occupation>)Enum.GetValues(typeof(Occupation)); }
        }

        public Occupation? SelectedOccupation
        {
            get { return _selectedOccupation; }
            set
            {
                _selectedOccupation = value;
                NotifyPropertyChanged("SelectedOccupation");
                AddDriverCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                NotifyPropertyChanged("DateOfBirth");
                AddDriverCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanAddDriver
        {
            get { return !string.IsNullOrEmpty(DriverName) && SelectedOccupation.HasValue && DateOfBirth.HasValue; }
        }

        public ObservableCollection<Driver> Drivers
        {
            get { return _drivers; }
        }

        public Driver SelectedDriver
        {
            get { return _selectedDriver; }
            set
            {
                _selectedDriver = value;
                NotifyPropertyChanged("SelectedDriver");
                NotifyPropertyChanged("SelectedDriverClaims");
            }
        }

        public IEnumerable<Claim> SelectedDriverClaims
        {
            get { return SelectedDriver != null ? SelectedDriver.Claims : new List<Claim>(); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddDriverCommand
        {
            get;
            private set;
        }

        public DelegateCommand ClearDriverDetailsCommand
        {
            get;
            private set;
        }
        #endregion Commands

        private void OnAddDriver()
        {
            Driver driver = new Driver
            {
                Name = DriverName,
                Occupation = SelectedOccupation.Value,
                DateOfBirth = DateOfBirth.Value,
                Claims = new List<Claim>()
            };

            _drivers.Add(driver);

            OnClearDriverDetails();
        }

        private void OnClearDriverDetails()
        {
            DriverName = null;
            SelectedOccupation = null;
            DateOfBirth = null;
        }
    }
}
