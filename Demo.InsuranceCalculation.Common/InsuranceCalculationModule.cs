using Demo.InsuranceCalculation.PolicyDeclination;
using Demo.InsuranceCalculation.PremiumCalculation;
using Demo.InsuranceCalculation.Services;
using Microsoft.Practices.Unity;
using Prism.Modularity;

namespace Demo.InsuranceCalculation
{
    public class InsuranceCalculationModule : IModule
    {
        private readonly IUnityContainer _container;

        public InsuranceCalculationModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
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
        }
    }
}
