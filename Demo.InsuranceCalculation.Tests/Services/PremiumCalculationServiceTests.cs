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

        [TestMethod]
        public void PremiumIsIncreasedByTenPercentIfThereIsADriverWhoIsAnAccountant()
        {
            InsurancePolicy policy = new InsurancePolicy(
                new DateTime(2016, 9, 16),
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
                new DateTime(2016, 9, 16),
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
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver { Name = "Joe", Occupation = Occupation.Chauffeur,  DateOfBirth = new DateTime(1995, 9, 16) },
                    new Driver { Name = "John", Occupation = Occupation.Chauffeur,  DateOfBirth = new DateTime(1990, 9, 16)}
                });

            decimal finalPremium = new DriverAgePremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = StartingPremium * 1.2m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremiumIsDecreasedByTenPercentIfYoungestDriverIsAgedBetween26And75()
        {
            InsurancePolicy policy = new InsurancePolicy(
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver { Name = "John", Occupation = Occupation.Chauffeur,  DateOfBirth = new DateTime(1990, 9, 16)},
                    new Driver { Name = "James", Occupation = Occupation.Chauffeur,  DateOfBirth = new DateTime(1985, 9, 16)}
                });

            decimal finalPremium = new DriverAgePremiumCalculationRule().CalculatePremium(policy, StartingPremium);
            decimal expectedPremium = StartingPremium * 0.9m;

            Assert.AreEqual(expectedPremium, finalPremium);
        }

        [TestMethod]
        public void PremimumIsIncreasedByTwentyPercentForEachClaimWithinOneYearOfStartOfPolicy()
        {
            InsurancePolicy policy = new InsurancePolicy(
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = new DateTime(1990, 9, 16),
                        Claims = new List<Claim> { new Claim { DateOfClaim = new DateTime(2016, 1, 16) }, new Claim { DateOfClaim = new DateTime(2015, 10, 16) } }
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
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = new DateTime(1990, 9, 16),
                        Claims = new List<Claim> { new Claim { DateOfClaim = new DateTime(2014, 9, 16) }, new Claim { DateOfClaim = new DateTime(2013, 9, 16) } }
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
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = new DateTime(1990, 9, 16),
                        Claims = new List<Claim> { new Claim { DateOfClaim = new DateTime(2016, 1, 16) }, new Claim { DateOfClaim = new DateTime(2013, 9, 16) } }
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
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = new DateTime(1990, 9, 16),
                        Claims = new List<Claim> { new Claim { DateOfClaim = new DateTime(2011, 9, 16) }, new Claim { DateOfClaim = new DateTime(2010, 9, 16) } }
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
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = new DateTime(1990, 9, 16)
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
                new DateTime(2016, 9, 16),
                new List<Driver>
                {
                    new Driver
                    {
                        Name = "John",
                        Occupation = Occupation.Chauffeur,
                        DateOfBirth = new DateTime(1990, 9, 16),
                        Claims = new List<Claim> { new Claim { DateOfClaim = new DateTime(2016, 1, 16) }, new Claim { DateOfClaim = new DateTime(2015, 10, 16) } }
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
