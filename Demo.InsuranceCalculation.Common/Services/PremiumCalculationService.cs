using System.Collections.Generic;
using System.Linq;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PremiumCalculation;

namespace Demo.InsuranceCalculation.Services
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationService"/> that calculates the total
    /// cost of a premium on an insurance policy with a starting premium of 500.
    /// </summary>
    public class PremiumCalculationService : IPremiumCalculationService
    {
        private const decimal StartingPremium = 500;
        private readonly IDictionary<int, IPremiumCalculationRule> _rulesByPrecedence;

        /// <summary>
        /// Initialises a new default instance of <see cref="PremiumCalculation"/>.
        /// </summary>
        public PremiumCalculationService()
        {
            _rulesByPrecedence = new Dictionary<int, IPremiumCalculationRule>();
        }

        /// <summary>
        /// Gets a collection of rules registered with this service.
        /// </summary>
        public IEnumerable<IPremiumCalculationRule> RegisteredRules
        {
            get { return _rulesByPrecedence.Values; }
        }

        #region IPremiumCalculationService Members
        public decimal CalculatePremium(InsurancePolicy policy)
        {
            decimal premium = StartingPremium;

            foreach(var rule in _rulesByPrecedence.Values.OrderBy(p => p.OrderOfPrecedence))
            {
                premium = rule.CalculatePremium(policy, premium);
            }
            return premium;
        }

        public void RegisterPremiumCalculationRule(IPremiumCalculationRule rule)
        {
            if(!_rulesByPrecedence.ContainsKey(rule.OrderOfPrecedence))
            {
                _rulesByPrecedence.Add(rule.OrderOfPrecedence, rule);
            }
        }
        #endregion IPremiumCalculationService Members
    }
}
