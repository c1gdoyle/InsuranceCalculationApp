using System;
using System.Collections.Generic;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PolicyDeclination;
using Demo.InsuranceCalculation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.InsuranceCalculation.Tests.Services
{
    [TestClass]
    public class PolicyDeclinationServiceTests
    {
        [TestMethod]
        public void ServiceDeclinesPolicyIfStartDateIsBeforeToday()
        {
            InsurancePolicy policy = new InsurancePolicy(
                DateTime.Today.AddDays(-1),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-25),
                    },
                });

            var service = CreateService();
            var result = service.AssessPolicy(policy);

            Assert.IsFalse(result.IsPolicyApproved);
            Assert.AreEqual("Start Date of Policy", result.Message);
        }

        [TestMethod]
        public void ServiceDeclinesPolicyIfYougestDriverIsUnder21()
        {
            InsurancePolicy policy = new InsurancePolicy(
                DateTime.Today.AddDays(1),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-20),
                    },
                    new Driver
                    {
                        Name = "Joe",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-30),
                    },
                });

            var service = CreateService();
            var result = service.AssessPolicy(policy);

            Assert.IsFalse(result.IsPolicyApproved);
            Assert.AreEqual("Age of Youngest Driver, John", result.Message);
        }

        [TestMethod]
        public void ServiceDeclinesPolicyIfOldestDriverIsOver75()
        {
            InsurancePolicy policy = new InsurancePolicy(
                DateTime.Today.AddDays(1),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-30),
                    },
                    new Driver
                    {
                        Name = "Joe",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-76),
                    },
                });

            var service = CreateService();
            var result = service.AssessPolicy(policy);

            Assert.IsFalse(result.IsPolicyApproved);
            Assert.AreEqual("Age of Oldest Driver, Joe", result.Message);
        }

        [TestMethod]
        public void ServiceDeclinesPolicyIfIndividualDriverHasMoreThanTwoClaims()
        {
            InsurancePolicy policy = new InsurancePolicy(
                DateTime.Today.AddDays(1),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-21),
                        Claims = new List<Claim> { new Claim(), new Claim(), new Claim() }
                    },
                });

            var service = CreateService();
            var result = service.AssessPolicy(policy);

            Assert.IsFalse(result.IsPolicyApproved);
            Assert.AreEqual("Driver has more than 2 claims, John", result.Message);
        }

        [TestMethod]
        public void ServiceDeclinesPolicyIfTotalNumberOfClaimsExceedsThree()
        {
            InsurancePolicy policy = new InsurancePolicy(
                DateTime.Today.AddDays(1),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-25),
                        Claims = new List<Claim> { new Claim(), new Claim()}
                    },
                    new Driver
                    {
                        Name = "Joe",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth =  DateTime.Today.AddYears(-25),
                        Claims = new List<Claim> { new Claim(), new Claim()}
                    },
                });

            var service = CreateService();
            var result = service.AssessPolicy(policy);

            Assert.IsFalse(result.IsPolicyApproved);
            Assert.AreEqual("Policy has more than 3 claims", result.Message);
        }

        [TestMethod]
        public void ServiceApprovesPolicyIfAllRulesArePassed()
        {
            InsurancePolicy policy = new InsurancePolicy(
                DateTime.Today.AddDays(1),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = DateTime.Today.AddYears(-25)
                    },
                    new Driver
                    {
                        Name = "Joe",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth =  DateTime.Today.AddYears(-25)
                    },
                });

            var service = CreateService();
            var result = service.AssessPolicy(policy);

            Assert.IsTrue(result.IsPolicyApproved);
        }

        private IPolicyDeclinationService CreateService()
        {
            var service = new PolicyDeclinationService();
            service.RegisterPolicyDeclinationRule(new StartDatePolicyDeclinationRule());
            service.RegisterPolicyDeclinationRule(new YoungestDriverAgePolicyDeclinationRule());
            service.RegisterPolicyDeclinationRule(new OldestDriverAgePolicyDeclinationRule());
            service.RegisterPolicyDeclinationRule(new IndividualDriverClaimsPolicyDeclinationRule());
            service.RegisterPolicyDeclinationRule(new TotalClaimsPolicyDeclinationRule());

            return service;
        }
    }
}
