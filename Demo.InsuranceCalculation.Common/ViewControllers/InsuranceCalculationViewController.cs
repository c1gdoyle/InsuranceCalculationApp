using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Services;
using Demo.Presentation.Base;
using Demo.Presentation.Commands;
using Demo.Presentation.Dialog;
using Prism.Commands;

namespace Demo.InsuranceCalculation.ViewControllers
{
    /// <summary>
    /// View controller for Insurance Calculation view.
    /// </summary>
    public class InsuranceCalculationViewController : ViewControllerBase
    {
        #region Private Members
        private readonly IInsurancePolicyAssessmentService _policyAssessmentService;
        private readonly IDialogService _dialogSerivce;

        private string _driverName;
        private Occupation? _selectedOccupation;
        private DateTime? _dateOfBirth;

        private readonly ObservableCollection<Driver> _drivers;
        private Driver _selectedDriver;

        private DateTime? _dateOfClaim;

        private DateTime? _policyStartDate;
        #endregion Private Members

        public InsuranceCalculationViewController(IInsurancePolicyAssessmentService policyAssessmentService, IDialogService dialogService)
        {
            _policyAssessmentService = policyAssessmentService;
            _dialogSerivce = dialogService;

            _drivers = new ObservableCollection<Driver>();

            InitialiseCommands();
        }

        private void InitialiseCommands()
        {
            // driver commands
            AddDriverCommand = new NotifiedDelegateCommand(() => OnAddDriverCommand(), () => CanAddDriver);
            ClearDriverDetailsCommand = new DelegateCommand(() => OnClearDriverDetailsCommand());

            //policy commands
            SubmitPolicyCommand = new NotifiedDelegateCommand(() => OnSubmitPolicyCommand(), () => CanSubmitPolicy);
            ResetPolicyCommand = new DelegateCommand(() => OnResetPolicyCommand());

            //claims command
            AddClaimCommand = new NotifiedDelegateCommand(() => OnAddClaimCommand(), () => CanAddClaim);

            //exit command
            ExitCommand = new DelegateCommand(() => NotifiyWindowShouldClose(null));
        }

        #region Properties
        public string DriverName
        {
            get { return _driverName; }
            set
            {
                _driverName = value;
                NotifyPropertyChanged("DriverName");
                NotifyPropertyChanged("CanAddDriver");
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
                NotifyPropertyChanged("CanAddDriver");
            }
        }

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                NotifyPropertyChanged("DateOfBirth");
                NotifyPropertyChanged("CanAddDriver");
            }
        }

        public bool CanAddDriver
        {
            get
            {
                return !string.IsNullOrEmpty(DriverName) &&
                    SelectedOccupation.HasValue &&
                    DateOfBirth.HasValue &&
                    Drivers.Count < 5;
            }
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
                NotifyPropertyChanged("CanAddClaim");
            }
        }

        public ObservableCollection<Claim> SelectedDriverClaims
        {
            get
            {
                if(SelectedDriver != null && SelectedDriver.Claims != null)
                {
                    return new ObservableCollection<Claim>(SelectedDriver.Claims);
                }
                else
                {
                    return new ObservableCollection<Claim>();
                }
            }
        }

        public DateTime? DateOfClaim
        {
            get { return _dateOfClaim; }
            set
            {
                _dateOfClaim = value;
                NotifyPropertyChanged("DateOfClaim");
                NotifyPropertyChanged("CanAddClaim");
            }
        }

        public bool CanAddClaim
        {
            get
            {
                return SelectedDriver != null && 
                    SelectedDriver.Claims != null && 
                    SelectedDriver.Claims.Count < 5 &&
                    DateOfClaim.HasValue;
            }
        }

        public DateTime? PolicyStartDate
        {
            get { return _policyStartDate; }
            set
            {
                _policyStartDate = value;
                NotifyPropertyChanged("PolicyStartDate");
                NotifyPropertyChanged("CanSubmitPolicy");
            }
        }

        public bool CanSubmitPolicy
        {
            get
            {
                return PolicyStartDate.HasValue &&
                  Drivers != null &&
                  Drivers.Count >= 1 &&
                  Drivers.Count <= 5;
            }
        }
        #endregion

        #region Commands
        public NotifiedDelegateCommand AddDriverCommand
        {
            get;
            private set;
        }

        public DelegateCommand ClearDriverDetailsCommand
        {
            get;
            private set;
        }

        public NotifiedDelegateCommand SubmitPolicyCommand
        {
            get;
            private set;
        }

        public DelegateCommand ResetPolicyCommand
        {
            get;
            private set;
        }

        public NotifiedDelegateCommand AddClaimCommand
        {
            get;
            private set;
        }

        public DelegateCommand ExitCommand
        {
            get;
            private set;
        }
        #endregion Commands

        private void OnAddDriverCommand()
        {
            Driver driver = new Driver
            {
                Name = DriverName,
                Occupation = SelectedOccupation.Value,
                DateOfBirth = DateOfBirth.Value,
                Claims = new List<Claim>()
            };

            _drivers.Add(driver);
            NotifyPropertyChanged("CanAddDriver");                
            NotifyPropertyChanged("CanSubmitPolicy");
        }

        private void OnClearDriverDetailsCommand()
        {
            DriverName = null;
            SelectedOccupation = null;
            DateOfBirth = null;
        }

        private void OnAddClaimCommand()
        {
            var claim = new Claim
            {
                DateOfClaim = DateOfClaim.Value
            };

            SelectedDriver.Claims.Add(claim);
            NotifyPropertyChanged("SelectedDriverClaims");
            NotifyPropertyChanged("CanAddClaim");
        }

        private void OnSubmitPolicyCommand()
        {
            InsurancePolicy policy = new InsurancePolicy(PolicyStartDate.Value, Drivers);

            var result = _policyAssessmentService.AssessPolicyApplication(policy);

            if(result.IsPolicyApproved)
            {
                _dialogSerivce.ShowMessage(string.Format("Insurance Policy approved. Premium : {0}", result.PremiumAmount));
            }
            else
            {
                _dialogSerivce.ShowMessage(result.Message, result.IsPolicyApproved);
            }
        }

        private void OnResetPolicyCommand()
        {
            SelectedDriver = null;
            Drivers.Clear();
            PolicyStartDate = null;
            NotifyPropertyChanged("CanAddDriver");
        }
    }
}
