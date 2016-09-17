using System;
using System.Collections.Generic;
using System.Linq;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PremiumCalculation;
using Demo.InsuranceCalculation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.InsuranceCalculation.Tests.Services
{
    [TestClass]
    public class PremiumCalculationServiceTests
    {
        private const decimal StartingPremium = 500.0m;
        private readonly DateTime PolicyStartDate = DateTime.Today.AddDays(1);

        [TestMethod]
        public void PremiumIsIncreasedByTenPercentIfThereIsADriverWhoIsAChauffeur()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver { Name = "Joe", Occupation = Occupation.Chauffeur }
                });
            decimal finalPremium = new DriverOccupationPremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = StartingPremium * 1.1m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsDecreasedByTenPercentIfThereIsADriverWhoIsAnAccountant()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver { Name = "Joe", Occupation = Occupation.Accountant }
                });

            decimal finalPremium = new DriverOccupationPremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = StartingPremium * 0.9m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsIncreasedByTwentyPercentIfYoungestDriverIsAgedBetween21And25()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver { Name = "Joe", Occupation = Occupation.Chauffeur,  DateOfBirth = PolicyStartDate.AddYears(-21) },
                    new Driver { Name = "John", Occupation = Occupation.Chauffeur,  DateOfBirth = PolicyStartDate.AddYears(-26)}
                });

            decimal finalPremium = new DriverAgePremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = StartingPremium * 1.2m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsDecreasedByTenPercentIfYoungestDriverIsAgedBetween26And75()
        {
            InsurancePolicy policy = new InsurancePolicy(
               PolicyStartDate,
                new List<Driver>
                {
                    new Driver { Name = "John", Occupation = Occupation.Chauffeur,  DateOfBirth = PolicyStartDate.AddYears(-26)},
                    new Driver { Name = "James", Occupation = Occupation.Chauffeur,  DateOfBirth = PolicyStartDate.AddYears(-30)}
                });

            decimal finalPremium = new DriverAgePremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = StartingPremium * 0.9m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremimumIsIncreasedByTwentyPercentForEachClaimWithinOneYearOfStartOfPolicy()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = PolicyStartDate.AddYears(-26),
                        Claims = new List<Claim> { new Claim { DateOfClaim = PolicyStartDate.AddMonths(-8) }, new Claim { DateOfClaim = PolicyStartDate.AddMonths(-11) } }
                    },                    
                });

            decimal finalPremium = new DriverClaimsPremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = (StartingPremium * 1.2m) * 1.2m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsIncreasedByTenPercentForEachClaimsWithinTwoToFiveYearsOfStartOfPolicy()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = PolicyStartDate.AddYears(-26),
                        Claims = new List<Claim> { new Claim { DateOfClaim = PolicyStartDate.AddYears(-2) }, new Claim { DateOfClaim = PolicyStartDate.AddYears(-3) } }
                    },
                });

            decimal finalPremium = new DriverClaimsPremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = (StartingPremium * 1.1m) * 1.1m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsIncreasedByTwentyPercentForClaimsWithinOneYearAndTenPercentForWithinTwoToFiveYears()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth =PolicyStartDate.AddYears(-26),
                        Claims = new List<Claim> { new Claim { DateOfClaim = PolicyStartDate.AddMonths(-8) }, new Claim { DateOfClaim = PolicyStartDate.AddYears(-3) } }
                    },
                });

            decimal finalPremium = new DriverClaimsPremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = (StartingPremium * 1.2m) * 1.1m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsNotAffectedByClaimsFiveYearsOrMoreBeforeStartOfPolicy()
        {
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = PolicyStartDate.AddYears(-26),
                        Claims = new List<Claim> { new Claim { DateOfClaim = PolicyStartDate.AddYears(-5), }, new Claim { DateOfClaim = PolicyStartDate.AddYears(-6) } }
                    },
                });

            decimal finalPremium = new DriverClaimsPremiumCalculationRule().CalculatePremium(policy, StartingPremium);

            Assert.AreEqual(StartingPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumCalculationServiceOnlyAllowsRuleToRegisteredOnce()
        {
            var service = new PremiumCalculationService();
            service.RegisterPremiumCalculationRule(new DriverAgePremiumCalculationRule());
            service.RegisterPremiumCalculationRule(new DriverClaimsPremiumCalculationRule());
            service.RegisterPremiumCalculationRule(new DriverClaimsPremiumCalculationRule());

            Assert.AreEqual(2, service.RegisteredRules.Count());
        }

        [TestMethod]
        public void PremimumCalculationServiceAppliesOccupationRuleThenAgeRule()
        {
            var service = CreateService();
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = PolicyStartDate.AddYears(-26)
                    },
                });

            decimal finalPremium = service.CalculatePremium(policy);
            decimal expectedPremium = (StartingPremium * 1.1m) * 0.9m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremimumCalculationServiceAppliesOccupationRuleThenAgeRuleThenClaimsRule()
        {
            var service = CreateService();
            InsurancePolicy policy = new InsurancePolicy(
                PolicyStartDate,
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = PolicyStartDate.AddYears(-26),
                        Claims = new List<Claim> { new Claim { DateOfClaim = PolicyStartDate.AddMonths(-8) }, new Claim { DateOfClaim = PolicyStartDate.AddMonths(-10) } }
                    },
                });

            decimal finalPremium = service.CalculatePremium(policy);
            decimal expectedPremium = (((StartingPremium * 1.1m) * 0.9m) * 1.2m )* 1.2m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        private IPremiumCalculationService CreateService()
        {
            PremiumCalculationService service = new PremiumCalculationService();

            service.RegisterPremiumCalculationRule(new DriverOccupationPremiumCalculationRule());
            service.RegisterPremiumCalculationRule(new DriverAgePremiumCalculationRule());
            service.RegisterPremiumCalculationRule(new DriverClaimsPremiumCalculationRule());

            return service;
        }
    }
}
