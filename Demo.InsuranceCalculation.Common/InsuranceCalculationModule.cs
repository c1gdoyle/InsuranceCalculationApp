using Demo.InsuranceCalculation.PolicyDeclination;
using Demo.InsuranceCalculation.PremiumCalculation;
using Demo.InsuranceCalculation.Services;
using Demo.InsuranceCalculation.Views;
using Demo.Presentation.Dialog;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace Demo.InsuranceCalculation
{
    public class InsuranceCalculationModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public InsuranceCalculationModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<IDialogService, DialogService>();

            // premium calculation rules
            _container.RegisterType<IPremiumCalculationService, PremiumCalculationService>(new ContainerControlledLifetimeManager());
            var premiumCalculationService = _container.Resolve<IPremiumCalculationService>();
            premiumCalculationService.RegisterPremiumCalculationRule(new DriverOccupationPremiumCalculationRule());
            premiumCalculationService.RegisterPremiumCalculationRule(new DriverAgePremiumCalculationRule());
            premiumCalculationService.RegisterPremiumCalculationRule(new DriverClaimsPremiumCalculationRule());
            
            // policy decline rules
            _container.RegisterType<IPolicyDeclinationService, PolicyDeclinationService>(new ContainerControlledLifetimeManager());
            var policyDeclinationService = _container.Resolve<IPolicyDeclinationService>();
            policyDeclinationService.RegisterPolicyDeclinationRule(new StartDatePolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new YoungestDriverAgePolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new OldestDriverAgePolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new IndividualDriverClaimsPolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new TotalClaimsPolicyDeclinationRule());

            _container.RegisterType<IInsurancePolicyAssessmentService, InsurancePolicyAssessmentSerivce>(new ContainerControlledLifetimeManager());
            var assessmentService = _container.Resolve<IInsurancePolicyAssessmentService>();

            _regionManager.RegisterViewWithRegion("MainRegion", typeof(InsuranceCalculationView));
        }
    }
}
