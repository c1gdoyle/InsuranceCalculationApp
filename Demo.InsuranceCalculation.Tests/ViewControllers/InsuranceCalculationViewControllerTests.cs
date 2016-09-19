using System;
using System.Collections.Generic;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PolicyDeclination;
using Demo.InsuranceCalculation.PremiumCalculation;
using Demo.InsuranceCalculation.Services;
using Demo.InsuranceCalculation.ViewControllers;
using Demo.Presentation.Dialog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Demo.InsuranceCalculation.Tests.ViewControllers
{
    [TestClass]
    public class InsuranceCalculationViewControllerTests
    {
        private string _message;
        private bool _success;

        [TestMethod]
        public void ControllerDeclinesIfStartOfPolicyBeforeToday()
        {
            var viewController = new InsuranceCalculationViewController(CreateAssessmentService(), MockDialogService().Object);

            viewController.Drivers.Add(new Driver());
            viewController.PolicyStartDate = DateTime.Today.AddDays(-1);

            viewController.SubmitPolicyCommand.Execute();

            Assert.IsFalse(_success);
            Assert.AreEqual("Start Date of Policy", _message);
        }

        [TestMethod]
        public void ControllerDeclinesIfYoungestDriverUnder21()
        {
            var viewController = new InsuranceCalculationViewController(CreateAssessmentService(), MockDialogService().Object);

            viewController.Drivers.Add(new Driver { Name = "Brian", DateOfBirth = DateTime.Today.AddYears(-20)});
            viewController.PolicyStartDate = DateTime.Today.AddDays(1);

            viewController.SubmitPolicyCommand.Execute();

            Assert.IsFalse(_success);
            Assert.AreEqual("Age of Youngest Driver, Brian", _message);
        }

        [TestMethod]
        public void ControllerDeclinesIfOldestDriverOver75()
        {
            var viewController = new InsuranceCalculationViewController(CreateAssessmentService(), MockDialogService().Object);

            viewController.Drivers.Add(new Driver { Name = "Brian", DateOfBirth = DateTime.Today.AddYears(-76) });
            viewController.PolicyStartDate = DateTime.Today.AddDays(1);

            viewController.SubmitPolicyCommand.Execute();

            Assert.IsFalse(_success);
            Assert.AreEqual("Age of Oldest Driver, Brian", _message);
        }

        [TestMethod]
        public void ControllerDeclinesIfIndividualDriverHasMoreThanTwoClaims()
        {
            var viewController = new InsuranceCalculationViewController(CreateAssessmentService(), MockDialogService().Object);

            viewController.Drivers.Add(
                new Driver { Name = "Brian", DateOfBirth = DateTime.Today.AddYears(-25) , Claims = new List<Claim> { new Claim(), new Claim(), new Claim()} });
            viewController.PolicyStartDate = DateTime.Today.AddDays(1);

            viewController.SubmitPolicyCommand.Execute();

            Assert.IsFalse(_success);
            Assert.AreEqual("Driver has more than 2 claims, Brian", _message);
        }

        [TestMethod]
        public void ControllerDeclinesIfTotalClaimsExceedsThree()
        {
            var viewController = new InsuranceCalculationViewController(CreateAssessmentService(), MockDialogService().Object);

            viewController.Drivers.Add(
                new Driver { Name = "Brian", DateOfBirth = DateTime.Today.AddYears(-25), Claims = new List<Claim> { new Claim(), new Claim() } });
            viewController.Drivers.Add(
                new Driver { Name = "John", DateOfBirth = DateTime.Today.AddYears(-25), Claims = new List<Claim> { new Claim(), new Claim() } });
            viewController.PolicyStartDate = DateTime.Today.AddDays(1);

            viewController.SubmitPolicyCommand.Execute();

            Assert.IsFalse(_success);
            Assert.AreEqual("Policy has more than 3 claims", _message);
        }

        [TestMethod]
        public void ControllerDisplaysSuccessfulPolicyWithPremium()
        {
            var viewController = new InsuranceCalculationViewController(CreateAssessmentService(), MockDialogService().Object);
            viewController.Drivers.Add(
                new Driver { Name = "Brian", Occupation = Occupation.Chauffeur, DateOfBirth = DateTime.Today.AddYears(-25) });

            viewController.PolicyStartDate = DateTime.Today.AddDays(1);

            viewController.SubmitPolicyCommand.Execute();

            Assert.IsTrue(_success);
            Assert.AreEqual("Insurance Policy approved. Premium : 660.00", _message);

        }

        private IInsurancePolicyAssessmentService CreateAssessmentService()
        {
            var calculationService = new PremiumCalculationService();
            calculationService.RegisterPremiumCalculationRule(new DriverOccupationPremiumCalculationRule());
            calculationService.RegisterPremiumCalculationRule(new DriverAgePremiumCalculationRule());
            calculationService.RegisterPremiumCalculationRule(new DriverClaimsPremiumCalculationRule());

            var policyDeclinationService = new PolicyDeclinationService();
            policyDeclinationService.RegisterPolicyDeclinationRule(new StartDatePolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new YoungestDriverAgePolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new OldestDriverAgePolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new IndividualDriverClaimsPolicyDeclinationRule());
            policyDeclinationService.RegisterPolicyDeclinationRule(new TotalClaimsPolicyDeclinationRule());

            var policyAssessmentService = new InsurancePolicyAssessmentSerivce(calculationService, policyDeclinationService);
            return policyAssessmentService;
        }

        private Mock<IDialogService> MockDialogService()
        {
            _message = null;
            _success = false;
            Mock<IDialogService> dialogService = new Mock<IDialogService>();

            //approved
            dialogService.Setup(ds => ds.ShowMessage(It.IsAny<string>()))
                .Callback<string>((s) =>
                {
                    _message = s;
                    _success = true;
                });

            //declined
            dialogService.Setup(ds => ds.ShowMessage(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback<string, bool>((s, b) =>
                {
                    _message = s;
                    _success = b;
                });

            return dialogService;
        }
    }
}
